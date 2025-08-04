using L.Bank.Accounts.Common;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

public sealed class GetAllAccountsQueryHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService)
    : RequestHandler<GetAllAccountsQuery, MbResult<List<Account>>>
{
    public override async Task<MbResult<List<Account>>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<List<Account>>(query.OwnerId);

        var accounts = await accountsRepository.GetAllAccountsAsync(query.OwnerId);
        var notClosedAccounts = accounts.Where(a => a.CloseDate == null).ToList();
        return MbResult.Success(notClosedAccounts);
    }
}