using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Requests.CreateAccountStatement;

public sealed record CreateAccountStatementRequest(Account Account, DateOnly StartDate, DateOnly EndDate) 
    : IRequest<AccountStatementVm>;