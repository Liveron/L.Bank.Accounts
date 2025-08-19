using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record MoneyCreditedIntegrationEvent(
    Guid AccountId, decimal Amount, string Currency, Guid OperationId)
    : IntegrationEvent("account.money.credited")
{
    /// <summary>
    /// Id счета, на который зачислены деньги.
    /// </summary>
    public Guid AccountId { get; set; } = AccountId;
    [UsedImplicitly] public decimal Amount { get; set; } = Amount;
    [UsedImplicitly] public string Currency { get; set; } = Currency;
    [UsedImplicitly] public Guid OperationId { get; set; } = OperationId;
}