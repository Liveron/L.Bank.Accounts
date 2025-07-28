using L.Bank.Accounts.Features.Accounts.Exceptions;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed class TransferCommandHandler(IAccountsRepository accountsRepository) 
    : IRequestHandler<TransferCommand>
{
    public Task Handle(TransferCommand request, CancellationToken _)
    {
        var accountToDebit = accountsRepository.GetAccount(request.FromAccountId);

        if (accountToDebit is null)
            throw new AccountNotFoundException(request.FromAccountId);

        var accountToCredit = accountsRepository.GetAccount(request.ToAccountId);

        if (accountToCredit is null)
            throw new AccountNotFoundException(request.ToAccountId);

        accountToDebit.Debit(request.Sum, accountToCredit.Id, request.Description);
        accountToCredit.Credit(request.Sum, accountToDebit.Id, request.Description);

        return Task.CompletedTask; 
    }
}
