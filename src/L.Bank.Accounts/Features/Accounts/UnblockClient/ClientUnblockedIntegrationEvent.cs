using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public sealed record ClientUnblockedIntegrationEvent(Guid ClientId) 
    : IntegrationEvent("account.client.unblocked")
{
    /// <summary>
    /// Id блокируемого клиента.
    /// </summary>
    [UsedImplicitly]
    public Guid ClientId { get; private init; } = ClientId;
}