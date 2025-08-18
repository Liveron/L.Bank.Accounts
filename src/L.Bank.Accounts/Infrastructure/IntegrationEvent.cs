namespace L.Bank.Accounts.Infrastructure;

public record IntegrationEvent
{
    public Guid EventId { get; private init; }
    public string OccuredAt { get; private init; } 
    public string? RoutingKey { get; private init; }

    public IntegrationEvent(string? routingKey = null) 
        : this(Guid.NewGuid(), DateTime.UtcNow.ToString("O"), routingKey)
    { }

    public IntegrationEvent(Guid eventId, string occuredAt, string? routingKey = null)
    {
        EventId = eventId;
        OccuredAt = occuredAt;
        RoutingKey = routingKey;
    }
}