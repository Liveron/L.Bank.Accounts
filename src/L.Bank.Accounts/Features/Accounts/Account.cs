using L.Bank.Accounts.Common;

namespace L.Bank.Accounts.Features.Accounts;

public sealed class Account
{
    public Guid Id { get; }
    public Guid OwnerId { get; private set; }
    public AccountType Type { get; }
    public decimal InterestRate { get; private set; }
    public string Currency { get; private set; }
    public decimal Balance { get; private set; }

    //private readonly List<Transaction> _transactions = [];
    //public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public List<Transaction> Transactions { get; set; } = [];
    public DateOnly OpenDate { get; }
    public DateOnly? CloseDate { get; private set; }
    public DateOnly? MaturityDate { get; private set; }
    public bool Frozen { get; private set; }

    // ReSharper disable once IdentifierTypo Небходимо для совместимости с БД
    public uint Xmin { get; init; }


    // ReSharper disable once ConvertToPrimaryConstructor Первичный конструктор менее читаем
    public Account(Guid id, Guid ownerId, AccountType type, decimal interestRate, 
        string currency, decimal balance, DateOnly openDate, DateOnly? closeDate, DateOnly? maturityDate)
    {
        Id = id; 
        OwnerId = ownerId;
        Type = type;
        InterestRate = interestRate;
        Currency = currency;
        Balance = balance;
        OpenDate = openDate;
        CloseDate = closeDate;
        MaturityDate = maturityDate;
    }

    private Account(Guid id, Guid ownerId, AccountTerms accountTerm, string currency, DateOnly openDate, DateOnly? maturityDate)
    {
        Id = id;
        OwnerId = ownerId;
        Type = accountTerm.AccountType;
        InterestRate = accountTerm.InterestRate;
        Currency = currency;
        MaturityDate = maturityDate;
        OpenDate = openDate;
    }

    public static MbResult<Account> New(
        Guid id, Guid ownerId, AccountTerms accountTerm, string currency, DateOnly? maturityDate = null, decimal sum = 0)
    {
        if (accountTerm.AccountType == AccountType.Deposit && maturityDate == null)
            return MbResult.Fail<Account>("Для депозитного счета необходимо указать дату погашения.");

        var accountCreationDate = DateOnly.FromDateTime(DateTime.UtcNow);
        if (maturityDate <= accountCreationDate)
            return MbResult.Fail<Account>("Дата погашения должна быть позже даты открытия счета.");

        if (sum < 0)
            return MbResult.Fail<Account>("Нельзя открыть счет с отрицательной начальной суммой.");

        var account = new Account(id, ownerId, accountTerm, currency, accountCreationDate, maturityDate);
        if (sum != 0)
            account.Credit(sum);

        return MbResult.Success(account);
    }

    public MbResult ChangeInterestRate(decimal newRate)
    {
        if (Type == AccountType.Checking)
            return MbResult.Fail("Нельзя изменить процентную ставку для текущего счета.");

        InterestRate = newRate;

        return MbResult.Success();
    }

    public MbResult ChangeMaturityDate(DateOnly newMaturityDate)
    {
        if (Type != AccountType.Deposit)
            return MbResult.Fail("Нельзя изменить дату погашения для счета, который не является депозитным.");
        if (newMaturityDate <= OpenDate)
            return MbResult.Fail("Дата погашения должна быть позже даты открытия счета.");

        MaturityDate = newMaturityDate;

        return MbResult.Success();
    }

    public decimal GetBalanceBefore(DateOnly date)
    {
        var transactions = Transactions.Where(a => DateOnly.FromDateTime(a.DateTime) < date);
        decimal balance = 0;
        foreach (var transaction in transactions)
        {
            if (transaction.IsDebit)
                balance -= transaction.Sum;
            else
                balance += transaction.Sum;
        }

        return balance;
    }

    public MbResult Debit(decimal sum, Guid? counterpartyAccountId, string? description)
    {
        if (Frozen)
            return MbResult.Fail("Нельзя снимать деньги с заблокированного счета.");

        if (sum > Balance)
            return MbResult.Fail("На балансе недостаточно денег.");

        Balance -= sum;

        var transaction = new Transaction(Id, TransactionType.Debit, 
            sum, counterpartyAccountId, description);
        AddTransaction(transaction);

        return MbResult.Success();
    }

    public void Credit(decimal sum, Guid? counterpartyAccountId = null, string? description = null)
    {
        Balance += sum;

        var transaction = new Transaction(Id, TransactionType.Credit,
            sum, counterpartyAccountId, description);
        AddTransaction(transaction);
    }

    public MbResult RegisterTransaction(
        decimal sum, TransactionType type, Guid? counterpartyAccountId = null, string? description = null)
    {
        if (type == TransactionType.Debit)
            return Debit(sum, counterpartyAccountId, description);

        Credit(sum, counterpartyAccountId, description);

        return MbResult.Success();
    }

    public void Block()
    {
        Frozen = true;
    }

    public void Unblock()
    {
        Frozen = false;
    }

    private void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
}