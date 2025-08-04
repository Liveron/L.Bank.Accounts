namespace L.Bank.Accounts.Features.Accounts;

public sealed record AccountStatementVm
{
    public required Guid AccountId { get; init; }
    public required AccountType AccountType { get; init; }
    public required decimal InterestRate { get; init; }
    public required string Currency { get; init; }
    public required DateOnly OpenDate { get; init; }
    public required DateOnly? MaturityDate { get; init; }
    public required List<AccountStatementTransactionVm> Transactions { get; init; }
}

public sealed record AccountStatementTransactionVm
{
    public required Guid Id { get; init; }
    public required TransactionType Type { get; init; }
    public required decimal Sum { get; init; }
    public Guid? CounterpartyAccountId { get; init; }
    public string? Description { get; init; }
    public required DateTime DateTime { get; init; }
    public required decimal CurrentBalance { get; init; }
}