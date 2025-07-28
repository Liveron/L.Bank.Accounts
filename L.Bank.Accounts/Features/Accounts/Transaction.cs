namespace L.Bank.Accounts.Features.Accounts;

public sealed class Transaction(
    Guid id,
    Guid accountId,
    TransactionType type,
    decimal sum,
    Guid? counterPartyAccountId = null,
    string? description = null)
{
    public Guid Id { get; private set; } = id;
    public Guid AccountId { get; private set; } = accountId;
    public Guid? CounterpartyAccountId { get; private set; } = counterPartyAccountId;
    public decimal Sum { get; private set; } = sum;
    public string? Description { get; private set; } = description;
    public DateTime DateTime { get; private set; } = DateTime.UtcNow;
    public TransactionType Type { get; } = type;
    public bool IsDebit => Type == TransactionType.Debit;
}
