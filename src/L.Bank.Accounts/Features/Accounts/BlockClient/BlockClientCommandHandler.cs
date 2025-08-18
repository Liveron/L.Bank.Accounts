using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed class BlockClientCommandHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<BlockClientCommand, MbResult>
{
    public override async Task<MbResult> Handle(BlockClientCommand request, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(request.ClientId))
            return ResultFactory.FailUserNotFound(request.ClientId);

        var accounts = await accountsRepository.GetAllAccountsAsync(request.ClientId);
        foreach (var account in accounts)
            account.Block();

        await accountsRepository.SaveChangesAsync();
        return ResultFactory.Success();
    }
}