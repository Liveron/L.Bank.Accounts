using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;


public sealed record OpenAccountCommand : IRequest<MbResult<Guid>>
{
    /// <summary>
    /// ID владельца счета
    /// </summary>
    public required Guid OwnerId { get; init; }
    /// <summary>
    /// Условия счета. Допустимые значения: "Надежный-6", "Бесплатный текущий"
    /// </summary>
    /// <example>Бесплатный текущий</example>
    public required string AccountTerms { get; init; }
    /// <summary>
    /// Валюта счета (ISO 4217)
    /// </summary>
    /// <example>RUB</example>
    public required string Currency { get; init; }
    /// <summary>
    /// Начальная сумма счета
    /// </summary>
    public decimal Sum { get; init; } = 0;
    /// <summary>
    /// Дата погашения счета (для срочных вкладов)
    /// </summary>
    public DateOnly? MaturityDate { get; init; }
}