using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

public sealed record CheckAccountExistsQuery : IRequest<MbResult<bool>>
{
    /// <summary>
    /// ID пользователя, которого проверяется наличие счета
    /// </summary>
    public required Guid OwnerId { get; init; }
    /// <summary>
    /// Тип счета, наличие которого нужно проверить
    /// </summary>
    public AccountType? AccountType { get; init; }
}