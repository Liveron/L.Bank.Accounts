using L.Bank.Accounts.Common.Exceptions;

namespace L.Bank.Accounts.Features.Accounts;

public sealed class Account(Guid id, Guid ownerId, AccountTerms accountTerm, string currency)
{
    public Guid Id { get; } = id;
    public Guid OwnerId { get; private set; } = ownerId;
    public AccountType Type { get; private set; } = accountTerm.AccountType;
    public decimal InterestRate { get; private set; } = accountTerm.InterestRate;
    public string Currency { get; private set; } = currency;
    public decimal Balance { get; private set; }

    private readonly List<Transaction> _transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public DateTime OpenDate { get; private set; } = DateTime.UtcNow;
    public DateTime? CloseDate { get; private set; }
    public void Debit(decimal sum, Guid? counterpartyAccountId, string? description)
    {
        if (sum > Balance)
            throw new DomainException("На балансе недостаточно денег.");

        Balance -= sum;

        var transaction = new Transaction(Guid.NewGuid(), Id, TransactionType.Debit, 
            sum, counterpartyAccountId, description);
        AddTransaction(transaction);
    }

    public void Credit(decimal sum, Guid? counterpartyAccountId, string? description)
    {
        Balance += sum;

        var transaction = new Transaction(Guid.NewGuid(), Id, TransactionType.Credit,
            sum, counterpartyAccountId, description);
        AddTransaction(transaction);
    }

    public void RegisterTransaction(decimal sum, TransactionType type, Guid? counterpartyAccountId, string? description)
    {
        if (type == TransactionType.Credit)
        {
            Credit(sum, counterpartyAccountId, description);
        }
        else
        {
            Debit(sum, counterpartyAccountId, description);
        }
    }

    private void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }
}