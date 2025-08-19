using L.Bank.Accounts.Common;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public class UnblockClientCommandHandler(
    IIdentityService identityService, IAccountsRepository accountsRepository)
    : RequestHandler<UnblockClientCommand, MbResult>
{
    public override async Task<MbResult> Handle(UnblockClientCommand request, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(request.ClientId))
            ResultFactory.FailUserNotFound(request.ClientId);

        var accounts = await accountsRepository.GetAllAccountsAsync(request.ClientId);
        foreach (var account in accounts)
            account.Unblock();

        await accountsRepository.SaveChangesAsync();
        return ResultFactory.Success();
    }
}