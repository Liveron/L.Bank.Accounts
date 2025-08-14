namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record AccountOpened
{
    public required Guid AccountId { get; init; }
}