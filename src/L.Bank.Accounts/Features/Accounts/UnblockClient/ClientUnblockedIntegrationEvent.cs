using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public sealed record ClientUnblockedIntegrationEvent(Guid ClientId) : IntegrationEvent
{
    /// <summary>
    /// Id блокируемого клиента.
    /// </summary>
    public Guid ClientId { get; private init; } = ClientId;
}