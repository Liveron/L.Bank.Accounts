using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed class ChangeInterestRateCommandHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<ChangeInterestRateCommand, MbResult>
{
    public override async Task<MbResult> Handle(ChangeInterestRateCommand command, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(command.OwnerId))
            ResultFactory.FailAccountNotFound(command.AccountId);

        var account = await accountsRepository.GetAccountAsync(command.AccountId, command.OwnerId);

        return account is not null 
            ? account.ChangeInterestRate(command.InterestRate) 
            : ResultFactory.FailAccountNotFound(command.AccountId);
    }
}
