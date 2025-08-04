using L.Bank.Accounts.Common;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;
using Mapster;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

public sealed class GetAllAccountsQueryHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService)
    : RequestHandler<GetAllAccountsQuery, MbResult<List<AccountVm>>>
{
    public override async Task<MbResult<List<AccountVm>>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<List<AccountVm>>(query.OwnerId);

        var accounts = await accountsRepository.GetAllAccountsAsync(query.OwnerId);
        var notClosedAccounts = accounts.Where(a => a.CloseDate == null);
        var accountVms = notClosedAccounts.Adapt<List<AccountVm>>();
        return MbResult.Success(accountVms);
    }
}