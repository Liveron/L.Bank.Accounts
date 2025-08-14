using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.GetAccountBalance;

public sealed class GetAccountBalanceQueryHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService)
    : RequestHandler<GetAccountBalanceQuery, MbResult<decimal>>
{
    public override async Task<MbResult<decimal>> Handle(GetAccountBalanceQuery query, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<decimal>(query.OwnerId);

        var account = await accountsRepository.GetAccountAsync(query.AccountId, query.OwnerId);

        return account is not null 
            ? ResultFactory.Success(account.Balance) 
            : ResultFactory.FailAccountNotFound<decimal>(query.AccountId);
    }
}