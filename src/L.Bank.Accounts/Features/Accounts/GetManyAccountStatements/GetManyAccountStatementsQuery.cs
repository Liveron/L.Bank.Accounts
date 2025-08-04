using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetManyAccountStatements;

public sealed record GetManyAccountStatementsQuery 
    : IRequest<MbResult<List<AccountStatementVm>>>
{
    public required Guid OwnerId { get; init; }
    public required IEnumerable<Guid>? AccountIds { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
}