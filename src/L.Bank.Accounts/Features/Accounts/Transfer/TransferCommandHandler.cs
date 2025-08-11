using System.Data;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Database;
using L.Bank.Accounts.Features.Accounts.Errors;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed class TransferCommandHandler(
    IAccountsRepository accountsRepository, AccountsDbContext dbContext) 
    : RequestHandler<TransferCommand, MbResult>
{
    public override async Task<MbResult> Handle(TransferCommand command, CancellationToken _)
    {
        await using var transaction = await dbContext.BeginTransactionAsync(IsolationLevel.Serializable);

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

        var accountToDebitAfterTransfer = await accountsRepository.GetAccountAsync(
            command.FromAccountId, command.FromAccountOwnerId);

        var accountToCreditAfterTransfer = await accountsRepository.GetAccountAsync(
            command.ToAccountId, command.ToAccountOwnerId);

        var sumAfterTransfer = accountToDebitAfterTransfer!.Balance + accountToCreditAfterTransfer!.Balance;

        if (sumAfterTransfer == sumBeforeTransfer)
        {
            dbContext.RollbackTransaction();
            return ResultFactory.FailTransferSumNotCorrect(sumBeforeTransfer, sumAfterTransfer);
        }

        await dbContext.CommitTransactionAsync(transaction!);
        return MbResult.Success();
    }
}
