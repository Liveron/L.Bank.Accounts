using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed class ChangeInterestRateCommandHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<ChangeInterestRateCommand, MbResult>
{
    public override async Task<MbResult> Handle(ChangeInterestRateCommand query, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            ResultFactory.FailAccountNotFound(query.AccountId);

        var account = await accountsRepository.GetAccountAsync(query.AccountId, query.OwnerId);

        if (account is null)
            return ResultFactory.FailAccountNotFound(query.AccountId);

        var result = account.ChangeInterestRate(query.InterestRate);

        await accountsRepository.SaveChangesAsync();

        return result;
    }
}
