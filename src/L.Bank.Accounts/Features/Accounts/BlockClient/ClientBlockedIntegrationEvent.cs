using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure.Integration;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed record ClientBlockedIntegrationEvent(Guid ClientId) : IntegrationEvent
{
    /// <summary>
    /// Id владельца заблокированного счета
    /// </summary>
    [UsedImplicitly]
    public Guid ClientId { get; private init; } = ClientId;
}