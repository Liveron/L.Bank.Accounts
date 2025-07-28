using FluentValidation;
using L.Bank.Accounts.Common.Behaviors;
using L.Bank.Accounts.Features.Accounts;
using L.Bank.Accounts.Identity;
using System.Reflection;

namespace L.Bank.Accounts.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddAppControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            });
    }

    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddSingleton<IAccountsRepository, AccountsRepository>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();

            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }
}