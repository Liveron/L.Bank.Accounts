using FluentValidation;
using L.Bank.Accounts.Common.Behaviors;
using L.Bank.Accounts.Features.Accounts;
using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.PostgreSql;
using L.Bank.Accounts.Common.Swagger;
using L.Bank.Accounts.Features.Accounts.AccrueAllInterests;
using L.Bank.Accounts.Features.Accounts.BlockClient;
using L.Bank.Accounts.Features.Accounts.IntegrationEvents;
using L.Bank.Accounts.Features.Accounts.UnblockClient;
using L.Bank.Accounts.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using L.Bank.Accounts.Infrastructure.Database;
using L.Bank.Accounts.Infrastructure.Database.Outbox;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.MassTransit;
using MassTransit.Transports.Fabric;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace L.Bank.Accounts.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddApplicationControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }

    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<MbResultOperationFilter>();

            options.AddKeycloakAuth();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        var connectionString = builder.Configuration.GetConnectionString("Postgres");
        builder.Services.AddDbContext<AccountsDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        builder.Services.AddMigrations<AccountsDbContext>();

        builder.Services.AddScoped<IOutboxProcessor, OutboxProcessor>();
        builder.Services.AddScoped<IAccrueAllInterestsJob, AccrueAllInterestsJob>();
        builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        builder.Services.AddHangfire(options =>
        {
            options.UsePostgreSqlStorage(o =>
            {
                o.UseNpgsqlConnection(connectionString);
            });
        });
        builder.Services.AddHangfireServer();

        builder.Services.AddTransactionLevelHandlerMap();
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "http://keycloak:8080/realms/dev-realm";
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireSignedTokens = false,
                    ValidateIssuer = false
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<ClientBlockedIntegrationEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h => {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.UseRawJsonSerializer();

                cfg.Message<IntegrationEventEnvelope<ClientUnblockedIntegrationEvent>>(e =>
                    e.SetEntityName("account.events"));

                cfg.Publish<IntegrationEventEnvelope<ClientUnblockedIntegrationEvent>>(e =>
                    e.ExchangeType = ExchangeType.Topic);

                cfg.Message<IntegrationEventEnvelope<AccountOpenedIntegrationEvent>>(e =>
                    e.SetEntityName("account.events"));

                cfg.Publish<IntegrationEventEnvelope<AccountOpenedIntegrationEvent>>(e =>
                    e.ExchangeType = ExchangeType.Topic);

                cfg.Message<IntegrationEventEnvelope<ClientBlockedIntegrationEvent>>(e =>
                    e.SetEntityName("account.events"));

                cfg.Publish<IntegrationEventEnvelope<ClientBlockedIntegrationEvent>>(e =>
                    e.ExchangeType = ExchangeType.Topic);

                cfg.ReceiveEndpoint("account.antifraud", c =>
                {
                    c.ConfigureConsumeTopology = false;

                    //c.Bind("account.antifraud", s =>
                    //{
                    //    s.ExchangeType = ExchangeType.Topic;
                    //});

                    c.ConfigureConsumer<ClientBlockedIntegrationEventConsumer>(context);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}