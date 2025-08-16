using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed class CreateTransactionCommandHandler(
    IAccountsRepository accountsRepository, IIdentityService identityService) 
    : RequestHandler<CreateTransactionCommand, MbResult>
{
    public override async Task<MbResult> Handle(CreateTransactionCommand command, CancellationToken _)
    {
        if (!await identityService.IdentifyUserAsync(command.OwnerId))
            return ResultFactory.FailUserNotFound(command.OwnerId);

        var account = await accountsRepository.GetAccountAsync(command.AccountId, command.OwnerId);

        if (account is null)
            return ResultFactory.FailAccountNotFound(command.AccountId);

        var result = account.RegisterTransaction(
            command.Sum, command.TransactionType, command.CounterpartyAccountId, command.Description);

        await accountsRepository.SaveChangesAsync();

        return result.IsFailure ? ResultFactory.Fail(result.Error!) : ResultFactory.Success();
    }
}