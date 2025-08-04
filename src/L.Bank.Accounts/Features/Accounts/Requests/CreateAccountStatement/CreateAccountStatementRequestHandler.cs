using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Requests.CreateAccountStatement;

public sealed class CreateAccountStatementRequestHandler 
    : IRequestHandler<CreateAccountStatementRequest, AccountStatementVm>
{
    public Task<AccountStatementVm> Handle(
        CreateAccountStatementRequest request, CancellationToken cancellationToken)
    {
        var account = request.Account;

        var transactions = account.Transactions
            .Where(t => DateOnly.FromDateTime(t.DateTime) >= request.StartDate &&
                        DateOnly.FromDateTime(t.DateTime) <= request.EndDate);

        var balance = account.GetBalanceBefore(request.StartDate);
        var transactionVms = transactions.Select(t => new AccountStatementTransactionVm
        {
            Id = t.Id,
            Type = t.Type,
            Sum = t.Sum,
            CounterpartyAccountId = t.CounterpartyAccountId,
            Description = t.Description,
            DateTime = t.DateTime,
            CurrentBalance = balance += t.IsDebit ? -t.Sum : t.Sum
        });

        var statementVm = new AccountStatementVm
        {
            AccountId = account.Id,
            AccountType = account.Type,
            InterestRate = account.InterestRate,
            Currency = account.Currency,
            OpenDate = account.OpenDate,
            MaturityDate = account.MaturityDate,
            Transactions = transactionVms.ToList()
        };

        return Task.FromResult(statementVm);
    }
}