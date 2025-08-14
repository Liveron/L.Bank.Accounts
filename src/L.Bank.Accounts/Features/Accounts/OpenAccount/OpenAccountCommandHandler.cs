using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;

public sealed class OpenAccountCommandHandler(
    ICurrencyService currencyService, IAccountsRepository accountsRepository, IIdentityService identityService) 
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
            return MbResult.Fail<Guid>(accountCreationResult.Error!);

        var createdAccountId = accountsRepository.AddAccount(accountCreationResult.Value!);
        await accountsRepository.SaveChangesAsync();

        return MbResult.Success(createdAccountId);
    }
}
