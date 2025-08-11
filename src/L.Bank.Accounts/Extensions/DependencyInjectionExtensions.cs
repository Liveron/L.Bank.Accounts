using FluentValidation;
using L.Bank.Accounts.Common.Behaviors;
using L.Bank.Accounts.Features.Accounts;
using L.Bank.Accounts.Identity;
using System.Reflection;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.PostgreSql;
using L.Bank.Accounts.Common.Swagger;
using L.Bank.Accounts.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
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
    }
}