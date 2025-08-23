using L.Bank.Accounts.Infrastructure.Integration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace L.Bank.Accounts.Infrastructure.Integration.Outbox;

public sealed record OutboxEventEntry
{
    public Guid Id { get; init; }
    public DateTime SavedAt { get; init; } = DateTime.UtcNow;
    public required string Event { get; init; }
    public required string EventType { get; init; }
    public OutboxEventEntryStatus Status { get; set; } = OutboxEventEntryStatus.NotPublished;
    [NotMapped]
    public IntegrationEvent? IntegrationEvent { get; private set; }
    public OutboxEventEntry DeserializeJsonEvent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Event, type) as IntegrationEvent;
        return this;
    }
}