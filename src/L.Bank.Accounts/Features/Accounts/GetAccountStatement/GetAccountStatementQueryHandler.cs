using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Features.Accounts.Requests.CreateAccountStatement;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAccountStatement;

public sealed class GetAccountStatementQueryHandler(
    IMediator mediator, IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<GetAccountStatementQuery, MbResult<AccountStatementVm>>
{
    public override async Task<MbResult<AccountStatementVm>> Handle(
        GetAccountStatementQuery query, CancellationToken token)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<AccountStatementVm>(query.OwnerId);

        var account = await accountsRepository.GetAccountAsync(query.AccountId, query.OwnerId);
        if (account is null)
            return ResultFactory.FailAccountNotFound<AccountStatementVm>(query.AccountId);

        var command = new CreateAccountStatementRequest(account, query.StartDate, query.EndDate);
        var statementVm = await mediator.Send(command, token);

        return ResultFactory.Success(statementVm);
    }
}
