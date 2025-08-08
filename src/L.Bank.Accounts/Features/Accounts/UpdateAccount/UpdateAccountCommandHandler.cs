using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;

namespace L.Bank.Accounts.Features.Accounts.UpdateAccount;

public class UpdateAccountCommandHandler(IAccountsRepository accountsRepository) 
    : RequestHandler<UpdateAccountCommand, MbResult>
{
    public override async Task<MbResult> Handle(UpdateAccountCommand command, CancellationToken token)
    {
        var account = await accountsRepository.GetAccountAsync(command.AccountId, command.OwnerId);
        if (account is null)
            return ResultFactory.FailAccountNotFound(command.AccountId);

        if (command.InterestRate.HasValue)
        {
            var result = account.ChangeInterestRate(command.InterestRate.Value);
            if (result.IsFailure)
                return result;
        }


        // ReSharper disable once ConvertIfStatementToReturnStatement Предлагает плохо читаемый код
        if (command.MaturityDate.HasValue)
            return account.ChangeMaturityDate(command.MaturityDate.Value);

        return MbResult.Success();
    }
}
