using JetBrains.Annotations;

namespace L.Bank.Accounts.Infrastructure;

public abstract record IntegrationEvent(Guid EventId, string OccuredAt, string? RoutingKey = null)
{
    public Guid EventId { get; private init; } = EventId;
    [UsedImplicitly] public string OccuredAt { get; private init; } = OccuredAt;
    public string? RoutingKey { get; private init; } = RoutingKey;

    protected IntegrationEvent(string? routingKey = null) 
        : this(Guid.NewGuid(), DateTime.UtcNow.ToString("O"), routingKey)
    { }
}