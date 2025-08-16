namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public sealed record OutboxEventEntry
{
    public Guid Id { get; init; }
    public string OccuredAt { get; init; } = DateTime.UtcNow.ToString("o");
    public required string Event { get; init; }
    public required string EventType { get; init; }
    public OutboxEventEntryStatus Status { get; set; } = OutboxEventEntryStatus.NotPublished;
}