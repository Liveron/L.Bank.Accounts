using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record AccountOpenedIntegrationEvent : IntegrationEvent
{
    public Guid AccountId { get; private init; }
    public Guid OwnerId { get; private init; }
    public string Currency { get; private init; }
    public AccountType Type { get; private init; }

    public AccountOpenedIntegrationEvent(
        Guid accountId, Guid ownerId, string currency, AccountType type) 
        : base("account.opened")
    {
        AccountId = accountId;
        OwnerId = ownerId;
        Currency = currency;
        Type = type;
    }
}