using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record AccountOpenedIntegrationEvent : IntegrationEvent
{
    public required Guid AccountId { get; init; }
    public required Guid OwnerId { get; init; }
    public required string Currency { get; init; }
    public required AccountType Type { get; init; }
}