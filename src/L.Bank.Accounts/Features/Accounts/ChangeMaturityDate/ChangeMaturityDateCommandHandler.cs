using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.ChangeMaturityDate;

public sealed class ChangeMaturityDateCommandHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<ChangeMaturityDateCommand, MbResult>
{
    public override async Task<MbResult> Handle(ChangeMaturityDateCommand command, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(command.OwnerId))
            return ResultFactory.FailUserNotFound(command.OwnerId);

        var account = await accountsRepository.GetAccountAsync(command.AccountId, command.OwnerId);

        return account is not null 
            ? account.ChangeMaturityDate(command.MaturityDate) 
            : ResultFactory.FailAccountNotFound(command.AccountId);
    }
}