using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetManyAccountStatements;

public sealed record GetManyAccountStatementsQuery 
    : IRequest<MbResult<List<AccountStatementVm>>>
{
    /// <summary>
    /// ID владельца счета
    /// </summary>
    public required Guid OwnerId { get; init; }
    /// <summary>
    /// Коллекция идентификаторов счетов
    /// </summary>
    public required IEnumerable<Guid>? AccountIds { get; init; }
    /// <summary>
    /// Начальная дата периода, за который запрашивается выписка
    /// </summary>
    public required DateOnly StartDate { get; init; }
    /// <summary>
    /// Конечная дата периода, за который запрашивается выписка
    /// </summary>
    public required DateOnly EndDate { get; init; }
}