using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public sealed class OutboxHealthCheck(IOutboxService outbox) : IHealthCheck
{
    private const int WarningThreshold = 100;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var notPublishedEvents = await outbox.GetNotPublishedEvents();

        if (notPublishedEvents.Count() > WarningThreshold)
        {
            return HealthCheckResult.Degraded(
                description: $"Outbox has more than {WarningThreshold} not published events.");
        }

        return HealthCheckResult.Healthy();
    }
}