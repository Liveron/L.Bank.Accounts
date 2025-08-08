using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts;

public sealed record AccountVm
{
    /// <summary>
    /// ID счета
    /// </summary>
    public required Guid Id { get; init; }
    /// <summary>
    /// Тип счета
    /// </summary>
    public required AccountType AccountType { get; init; }
    /// <summary>
    /// Процентная ставка
    /// </summary>
    [UsedImplicitly]
    public required decimal InterestRate { get; init; }
    /// <summary>
    /// Валюта
    /// </summary>
    [UsedImplicitly]
    public required string Currency { get; init; }
    /// <summary>
    /// Дата открытия
    /// </summary>
    [UsedImplicitly]
    public required DateOnly OpenDate { get; init; }
    /// <summary>
    /// Дата погашения счета (для депозитов)
    /// </summary>
    [UsedImplicitly]
    public required DateOnly? MaturityDate { get; init; }
}