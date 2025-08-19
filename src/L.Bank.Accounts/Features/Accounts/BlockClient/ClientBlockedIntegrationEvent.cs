using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed record ClientBlockedIntegrationEvent(Guid ClientId) 
    : IntegrationEvent("account.client.blocked")
{
    /// <summary>
    /// Id владельца заблокированного счета
    /// </summary>
    [UsedImplicitly]
    public Guid ClientId { get; private init; } = ClientId;
}