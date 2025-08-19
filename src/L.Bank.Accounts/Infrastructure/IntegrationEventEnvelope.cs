using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Infrastructure;

public sealed record IntegrationEventEnvelope<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    public TIntegrationEvent Payload { get; private init; }
    public IntegrationEventEnvelopeMetadata Meta { get; private init; }

    [JsonConstructor]
    public IntegrationEventEnvelope(TIntegrationEvent payload, IntegrationEventEnvelopeMetadata meta)
    {
        Payload = payload;
        Meta = meta;
    }

    public IntegrationEventEnvelope(
        TIntegrationEvent @event, string version , string source, Guid correlationId, Guid causationId = default)
    {
        Payload = @event;
        Meta = new IntegrationEventEnvelopeMetadata
        {
            Version = version,
            Source = source,
            CorrelationId = correlationId,
            CausationId = causationId
        };
    }
}

public sealed record IntegrationEventEnvelopeMetadata
{
    public string Version { get; init; } = "v1";
    public string Source { get; init; } = "account-service";
    public Guid CorrelationId { get; init; }
    public Guid CausationId { get; init; }
}