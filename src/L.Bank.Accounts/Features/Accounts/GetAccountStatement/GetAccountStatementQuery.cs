using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAccountStatement;

public sealed record GetAccountStatementQuery(Guid AccountId, Guid OwnerID, DateOnly StartDate, DateOnly EndDate) 
    : IRequest<MbResult<AccountStatementVm>>;