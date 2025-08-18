using MassTransit;

namespace L.Bank.Accounts.Infrastructure;

public interface IEnvelopeConsumer<TIntegrationEvent> : IConsumer<IntegrationEventEnvelope<TIntegrationEvent>>
    where TIntegrationEvent : IntegrationEvent;