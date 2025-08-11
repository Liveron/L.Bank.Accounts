using L.Bank.Accounts.Database;

namespace L.Bank.Accounts.Features.Accounts;

public interface IAccountsRepository
{
    public Guid AddAccount(Account account);
    public Task<Account?> GetAccountAsync(Guid accountId, Guid ownerId);
    public Task<List<Account>> GetAllAccountsAsync(Guid? userId);
    public Task SaveChangesAsync();
}

public sealed class AccountsRepository(AccountsDbContext dbContext) : IAccountsRepository
{
    private readonly List<Account> _accounts = [];

    public Guid AddAccount(Account account)
    {
        _accounts.Add(account);
        return account.Id;
    }

    public Task<Account?> GetAccountAsync(Guid accountId, Guid ownerId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId && a.OwnerId == ownerId);
        return Task.FromResult(account);
    }

    public Task<List<Account>> GetAllAccountsAsync(Guid? userId)
    {
        if (!userId.HasValue) 
            return Task.FromResult(_accounts);

        var accounts =_accounts.Where(a => a.OwnerId == userId).ToList();
        return Task.FromResult(accounts);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}