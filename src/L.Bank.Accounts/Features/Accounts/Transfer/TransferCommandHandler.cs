using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed class TransferCommandHandler(IAccountsRepository accountsRepository) 
    : RequestHandler<TransferCommand, MbResult>
{
    public override async Task<MbResult> Handle(TransferCommand command, CancellationToken _)
    {
        var accountToDebit = await accountsRepository.GetAccountAsync(command.FromAccountId, command.FromAccountOwnerId);
        if (accountToDebit is null)
            return ResultFactory.FailAccountNotFound(command.FromAccountId);

        var accountToCredit = await accountsRepository.GetAccountAsync(command.ToAccountId, command.ToAccountOwnerID);
        if (accountToCredit is null)
            return ResultFactory.FailAccountNotFound(command.ToAccountId);

        var debitResult = accountToDebit.Debit(command.Sum, accountToCredit.Id, command.Description);
        if (debitResult.IsFailure)
            return debitResult;

        accountToCredit.Credit(command.Sum, accountToDebit.Id, command.Description);

        return MbResult.Success();
    }
}
