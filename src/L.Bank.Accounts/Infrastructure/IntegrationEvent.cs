namespace L.Bank.Accounts.Infrastructure;

public record IntegrationEvent
{
    public Guid EventId { get; init; }
    public string OccuredAt { get; init; } = DateTime.UtcNow.ToString("O");
}