using System.Data;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Attributes;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Infrastructure.Database;
using L.Bank.Accounts.Infrastructure.Database.Outbox;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

[Transaction(IsolationLevel.Serializable)]
public sealed class TransferCommandHandler(
    IAccountsRepository accountsRepository, AccountsDbContext dbContext) 
    : RequestHandler<TransferCommand, MbResult>
{
    public override async Task<MbResult> Handle(TransferCommand command, CancellationToken _)
    {
        var accountToDebit = await accountsRepository.GetAccountAsync(command.FromAccountId, command.FromAccountOwnerId);
        if (accountToDebit is null)
            return ResultFactory.FailAccountNotFound(command.FromAccountId);

        var accountToCredit = await accountsRepository.GetAccountAsync(command.ToAccountId, command.ToAccountOwnerId);
        if (accountToCredit is null)
            return ResultFactory.FailAccountNotFound(command.ToAccountId);

        var sumBeforeTransfer = accountToDebit.Balance + accountToCredit.Balance;

        var debitResult = accountToDebit.Debit(command.Sum, accountToCredit.Id, command.Description);
        if (debitResult.IsFailure)
            return debitResult;

        accountToCredit.Credit(command.Sum, accountToDebit.Id, command.Description);

        await accountsRepository.SaveChangesAsync();

        var sumAfterTransfer = accountToDebit.Balance + accountToCredit.Balance;

        if (sumAfterTransfer == sumBeforeTransfer) 
            return ResultFactory.Success();

        dbContext.RollbackTransaction();
        return ResultFactory.FailTransferSumNotCorrect(sumBeforeTransfer, sumAfterTransfer);
    }
}
