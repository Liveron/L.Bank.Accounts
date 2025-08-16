using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.AccrueInterest;
using L.Bank.Accounts.Infrastructure.Database;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

public sealed class CloseAccountCommandHandler(
    AccountsDbContext dbContext, IIdentityService identityService, IMediator mediator) 
    : RequestHandler<CloseAccountCommand, MbResult>
{
    public override async Task<MbResult> Handle(CloseAccountCommand command, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(command.OwnerId))
            return ResultFactory.FailUserNotFound(command.OwnerId);

        var accrueInterestCommand = new AccrueInterestCommand(command.AccountId);
        return await mediator.Send(accrueInterestCommand, cancellationToken);
    }
}