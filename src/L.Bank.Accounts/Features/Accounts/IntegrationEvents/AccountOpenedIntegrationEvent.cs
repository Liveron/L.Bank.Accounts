using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record AccountOpenedIntegrationEvent(Guid AccountId, Guid OwnerId, string Currency, AccountType Type)
    : IntegrationEvent("account.opened")
{
    public Guid AccountId { get; private init; } = AccountId;
    public Guid OwnerId { get; private init; } = OwnerId;
    [UsedImplicitly] public string Currency { get; private init; } = Currency;
    public AccountType Type { get; private init; } = Type;
}