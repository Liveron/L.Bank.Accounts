using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAccountBalance;

public sealed record GetAccountBalanceQuery(Guid AccountId, Guid OwnerId) 
    : IRequest<MbResult<decimal>>;