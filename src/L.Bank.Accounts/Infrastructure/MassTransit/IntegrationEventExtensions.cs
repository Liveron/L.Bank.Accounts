using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public static class IntegrationEventExtensions
{
    public static async Task PublishIntegrationEvent<TIntegrationEvent>(
        this IPublishEndpoint endpoint, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent
    {
        var integrationEvent = new IntegrationEventEnvelope<TIntegrationEvent>(
            @event: @event,
            version: "v1",
            source: "account-service",
            correlationId: Guid.NewGuid());

        using var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        try
        {
            await endpoint.Publish(integrationEvent, context =>
            {
                context.SetRoutingKey(@event.RoutingKey);
                context.MessageId = @event.EventId;
                context.Headers.Set("EventType", @event.GetType().Name);
                context.Headers.Set("Version", integrationEvent.Meta.Version);
                context.Headers.Set("X-Correlation-Id", Guid.NewGuid());
                context.Headers.Set("X-Causation-Id", Guid.Empty);
            }, cancellationToken: source.Token);
        }
        catch (Exception)
        {
            throw new TimeoutException(
                $"Publishing integration event {@event.GetType().Name} timed out.");
        }
    }
}