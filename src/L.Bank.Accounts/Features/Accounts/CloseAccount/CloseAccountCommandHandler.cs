using L.Bank.Accounts.Common;
using L.Bank.Accounts.Database;
using L.Bank.Accounts.Features.Accounts.AccrueInterest;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;
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

        await using var transaction = await dbContext.BeginTransactionAsync();

        var accrueInterestCommand = new AccrueInterestCommand(command.AccountId);
        var result = await mediator.Send(accrueInterestCommand, cancellationToken);

        await transaction!.CommitAsync(cancellationToken);

        return result;
    }
}