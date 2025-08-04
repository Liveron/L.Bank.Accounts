namespace L.Bank.Accounts.Features.Accounts;

public sealed record AccountVm
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
}