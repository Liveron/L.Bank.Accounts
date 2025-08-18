using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public static class IntegrationEventExtensions
{
    public static Task PublishIntegrationEvent<TIntegrationEvent>(
        this IPublishEndpoint endpoint, TIntegrationEvent @event)
        where TIntegrationEvent : IntegrationEvent
    {
        var integrationEvent = new IntegrationEventEnvelope<TIntegrationEvent>(
            @event: @event,
            version: "v1",
            source: "account-service",
            correlationId: Guid.NewGuid());

        return endpoint.Publish(integrationEvent, context => context.SetRoutingKey(@event.RoutingKey));
    }
}