using JetBrains.Annotations;

namespace L.Bank.Accounts.Infrastructure.Integration;

public abstract record IntegrationEvent(Guid EventId, string OccuredAt)
{
    public Guid EventId { get; private init; } = EventId;
    [UsedImplicitly] public string OccuredAt { get; private init; } = OccuredAt;

    protected IntegrationEvent()
        : this(Guid.NewGuid(), DateTime.UtcNow.ToString("O"))
    { }
}