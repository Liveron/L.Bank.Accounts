using Hangfire;
using L.Bank.Accounts.Common.Middlewares;
using L.Bank.Accounts.Features.Accounts.AccrueAllInterests;
using L.Bank.Accounts.Infrastructure.Database.Outbox;

namespace L.Bank.Accounts.Extensions;

public static class ApplicationExtensions
{
    public static void UseSwaggerOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId("swagger-ui");
        });
    }

    public static void UseMbResultAuthorization(this WebApplication app)
    {
        app.UseMiddleware<MbResultUnauthorizedMiddleware>();
    }

    public static void UseBackgroundJobs(this WebApplication app)
    {
        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IAccrueAllInterestsJob>(
                "accrue-all-interests", job => job.ExecuteAsync(), Cron.Daily);

        app.Services.GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IOutboxProcessor>(
                "outbox-processor", processor => processor.ExecuteAsync(), "*/10 * * * * *");
    }
}
