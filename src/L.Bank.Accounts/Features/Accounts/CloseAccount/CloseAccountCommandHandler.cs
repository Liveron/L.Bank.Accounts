using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

public sealed class CloseAccountCommandHandler(
    IAccountsRepository accounts, IIdentityService identityService) 
    : RequestHandler<CloseAccountCommand, MbResult>
{
    public override async Task<MbResult> Handle(CloseAccountCommand command, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(command.OwnerId))
            return ResultFactory.FailUserNotFound(command.OwnerId);

        var account = await accounts.GetAccountAsync(command.AccountId, command.OwnerId);
        return account is not null ? account.Close() : ResultFactory.FailAccountNotFound(command.AccountId);
    }
}