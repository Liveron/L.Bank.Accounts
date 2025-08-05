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

        var transactionVms = new List<AccountStatementTransactionVm>();

        var balance = account.GetBalanceBefore(request.StartDate);
        foreach (var transaction in transactions)
        {
            var transactionVm = new AccountStatementTransactionVm
            {
                Id = transaction.Id,
                Type = transaction.Type,
                Sum = transaction.Sum,
                CounterpartyAccountId = transaction.CounterpartyAccountId,
                Description = transaction.Description,
                DateTime = transaction.DateTime,
                CurrentBalance = balance
            };
            transactionVms.Add(transactionVm);
            balance += transaction.IsDebit ? -transaction.Sum : transaction.Sum;
        }

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