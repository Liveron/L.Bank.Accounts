namespace L.Bank.Accounts.Features.Accounts;

public sealed record AccountStatementVm
{
    /// <summary>
    /// ID счета
    /// </summary>
    public required Guid AccountId { get; init; }
    /// <summary>
    /// Тип счета
    /// </summary>
    public required AccountType AccountType { get; init; }
    /// <summary>
    /// Процентная ставка
    /// </summary>
    public required decimal InterestRate { get; init; }
    /// <summary>
    /// Валюта
    /// </summary>
    public required string Currency { get; init; }
    /// <summary>
    /// Дата открытия
    /// </summary>
    public required DateOnly OpenDate { get; init; }
    /// <summary>
    /// Дата погашения счета (для депозитов)
    /// </summary>
    public required DateOnly? MaturityDate { get; init; }
    /// <summary>
    /// Коллекция транзакций
    /// </summary>
    public required List<AccountStatementTransactionVm> Transactions { get; init; }
}

public sealed record AccountStatementTransactionVm
{
    /// <summary>
    /// ID транзакции
    /// </summary>
    public required Guid Id { get; init; }
    /// <summary>
    /// Тип транзакции
    /// </summary>
    public required TransactionType Type { get; init; }
    /// <summary>
    /// Сумма транзакции
    /// </summary>
    public required decimal Sum { get; init; }
    /// <summary>
    /// ID контрагента
    /// </summary>
    public Guid? CounterpartyAccountId { get; init; }
    /// <summary>
    /// Описание транзакции
    /// </summary>
    public string? Description { get; init; }
    /// <summary>
    /// Время транзакции
    /// </summary>
    public required DateTime DateTime { get; init; }
    /// <summary>
    /// Баланс на момент транзакции
    /// </summary>
    public required decimal CurrentBalance { get; init; }
}