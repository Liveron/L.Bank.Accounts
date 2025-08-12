namespace L.Bank.Accounts.Features.Accounts;

public sealed class Transaction(
    Guid accountId,
    TransactionType type,
    decimal sum,
    Guid? counterpartyAccountId = null,
    string? description = null)
{
    public Guid Id { get; init; }
    // ReSharper disable once UnusedMember.Global Необходимо по заданию
    public Guid AccountId { get; init; } = accountId;
    public Guid? CounterpartyAccountId { get; init; } = counterpartyAccountId;
    public decimal Sum { get; init; } = sum;
    public string? Description { get; init; } = description;
    public DateTime DateTime { get; init; } = DateTime.UtcNow;
    public TransactionType Type { get; init; } = type;
    public bool IsDebit => Type == TransactionType.Debit;
}
