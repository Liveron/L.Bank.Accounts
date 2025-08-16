using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Features.Accounts.IntegrationEvents;
using L.Bank.Accounts.Infrastructure.Database.Outbox;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;

public sealed class OpenAccountCommandHandler(
    ICurrencyService currencyService, IAccountsRepository accountsRepository, 
    IIdentityService identityService, IOutboxService outboxService) 
    : RequestHandler<OpenAccountCommand, MbResult<Guid>>
{
    public override async Task<MbResult<Guid>> Handle(OpenAccountCommand request, CancellationToken _)
    {
        if (!await identityService.IdentifyUserAsync(request.OwnerId))
            return ResultFactory.FailUserNotFound<Guid>(request.OwnerId);

        if (!currencyService.CheckCurrency(request.Currency))
            return ResultFactory.FailCurrencyNotSupported<Guid>(request.Currency);

        var accountTerms = AccountTerms.FromName(request.AccountTerms);
        var accountCreationResult = Account.New(
            Guid.NewGuid(), request.OwnerId, accountTerms, request.Currency, request.MaturityDate, request.Sum);

        if (accountCreationResult.IsFailure)
            return ResultFactory.Fail<Guid>(accountCreationResult.Error!);

        var createdAccount = accountsRepository.AddAccount(accountCreationResult.Value!);
        await accountsRepository.SaveChangesAsync();

        var integrationEvent = new AccountOpenedIntegrationEvent
        {
            AccountId = createdAccount.Id,
            Currency = createdAccount.Currency,
            OwnerId = createdAccount.OwnerId,
            Type = createdAccount.Type
        };
        await outboxService.SaveEventAsync(integrationEvent);

        return ResultFactory.Success(createdAccount.Id);
    }
}
