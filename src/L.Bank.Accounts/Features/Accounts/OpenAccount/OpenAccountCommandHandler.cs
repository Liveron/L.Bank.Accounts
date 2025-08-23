using L.Bank.Accounts.Application;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Features.Accounts.IntegrationEvents;
using L.Bank.Accounts.Infrastructure.Identity;
using L.Bank.Accounts.Infrastructure.Identity.Errors;
using L.Bank.Accounts.Infrastructure.Integration;
using Npgsql.PostgresTypes;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;

public sealed class OpenAccountCommandHandler(
    ICurrencyService currencyService, IAccountsRepository accountsRepository, 
    IIdentityService identityService, IIntegrationService<IntegrationEvent> integrationService) 
    : RequestHandler<OpenAccountCommand, MbResult<Guid>>
{
    public override async Task<MbResult<Guid>> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
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

        var integrationEvent = new AccountOpenedIntegrationEvent(
            createdAccount.Id, createdAccount.OwnerId, createdAccount.Currency, createdAccount.Type);
        await integrationService.AddAndSaveAsync(integrationEvent);

        return ResultFactory.Success(createdAccount.Id);
    }
}
