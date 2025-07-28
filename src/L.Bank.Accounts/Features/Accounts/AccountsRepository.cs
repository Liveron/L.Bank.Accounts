namespace L.Bank.Accounts.Features.Accounts;

public interface IAccountsRepository
{
    public Guid AddAccount(Account account);
    public Account? GetAccount(Guid accountId);
}

public sealed class AccountsRepository : IAccountsRepository
{
    private readonly List<Account> _accounts = [];

    public Guid AddAccount(Account account)
    {
        _accounts.Add(account);
        return account.Id;
    }

    public Account? GetAccount(Guid accountId)
    {
        return _accounts.FirstOrDefault(a => a.Id == accountId);
    }
}