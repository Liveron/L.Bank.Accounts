using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Errors;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Features.Accounts.IntegrationEvents;
using L.Bank.Accounts.Infrastructure;
using L.Bank.Accounts.Infrastructure.Database.Outbox;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed class CreateTransactionCommandHandler(IOutboxService outbox,
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

        if (!result.IsSuccess) 
            return ResultFactory.FailConflict(result.Error!.Messages.First());

        IntegrationEvent @event = command.TransactionType == TransactionType.Credit
            ? new MoneyCreditedIntegrationEvent(
                command.AccountId, command.Sum, account.Currency, Guid.NewGuid())
            : new MoneyDebitedIntegrationEvent(
                command.AccountId, command.Sum, account.Currency, Guid.NewGuid(), command.Description);
        await outbox.SaveEventAsync(@event);

        return ResultFactory.Success();
    }
}