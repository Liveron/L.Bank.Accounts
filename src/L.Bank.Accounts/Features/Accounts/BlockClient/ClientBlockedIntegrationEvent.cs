using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed record ClientBlockedIntegrationEvent(Guid ClientId) 
    : IntegrationEvent("account.client.blocked")
{
    /// <summary>
    /// Id владельца заблокированного счета
    /// </summary>
    public Guid ClientId { get; private init; } = ClientId;
}