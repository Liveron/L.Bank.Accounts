using L.Bank.Accounts.Features.Accounts.Exceptions;
using L.Bank.Accounts.Identity;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;

public sealed class OpenAccountCommandHandler(
    ICurrencyService currencyService, IAccountsRepository accountsRepository, IIdentityService identityService) 
    : IRequestHandler<OpenAccountCommand, Guid>
{
    public Task<Guid> Handle(OpenAccountCommand request, CancellationToken _)
    {
        if (!identityService.IdentifyUser(request.OwnerId))
            throw new UserNotFoundException(request.OwnerId);

        if (!currencyService.CheckCurrency(request.Currency))
            throw new CurrencyNotSupportedException(request.Currency);

        var accountTerms = AccountTerms.FromName(request.AccountTerms);
        var account = new Account(Guid.NewGuid(), request.OwnerId, accountTerms, request.Currency);

        var createdAccountId = accountsRepository.AddAccount(account);

        return Task.FromResult(createdAccountId);
    }
}
