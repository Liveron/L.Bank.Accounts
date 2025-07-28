using L.Bank.Accounts.Features.Accounts.Exceptions;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed class CreateTransactionCommandHandler(IAccountsRepository accountsRepository) 
    : IRequestHandler<CreateTransactionCommand>
{
    public Task Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = accountsRepository.GetAccount(request.AccountId);

        if (account is null)
            throw new AccountNotFoundException(request.AccountId);

        account.RegisterTransaction(request.Sum, request.TransactionType, request.CounterpartyAccountId, request.Description);

        return Task.CompletedTask;
    }
}