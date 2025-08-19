using JetBrains.Annotations;
using L.Bank.Accounts.Infrastructure;

namespace L.Bank.Accounts.Features.Accounts.IntegrationEvents;

public sealed record MoneyDebitedIntegrationEvent(
    Guid AccountId,
    decimal Amount,
    string Currency,
    Guid OperationId,
    string? Reason)
    : IntegrationEvent("account.money.debited")
{
    /// <summary>
    /// Id счета, с которого списаны деньги.
    /// </summary>
    public Guid AccountId { get; set; } = AccountId;

    [UsedImplicitly] public decimal Amount { get; set; } = Amount;
    [UsedImplicitly] public string Currency { get; set; } = Currency;
    [UsedImplicitly] public Guid OperationId { get; set; } = OperationId;
    [UsedImplicitly] public string? Reason { get; set; } = Reason;
}
