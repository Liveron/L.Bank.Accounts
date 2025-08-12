using L.Bank.Accounts.Common.Exceptions;
using L.Bank.Accounts.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
    public Guid AddAccount(Account account)
    {
        dbContext.Accounts.Add(account);
        return account.Id;
    }

    public async Task<Account?> GetAccountAsync(Guid accountId, Guid ownerId)
    {
        return await dbContext.Accounts.Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == accountId && a.OwnerId == ownerId);
    }

    public async Task<List<Account>> GetAllAccountsAsync(Guid? userId)
    {
        if (!userId.HasValue) 
            return await dbContext.Accounts.Include(a => a.Transactions)
                .AsNoTracking()
                .ToListAsync();

        return await dbContext.Accounts.Include(a => a.Transactions)
            .AsNoTracking()
            .Where(a => a.OwnerId == userId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.InnerException is DbUpdateException 
                    { InnerException: PostgresException { SqlState: PostgresErrorCodes.SerializationFailure } } 
                || ex is DbUpdateConcurrencyException)
                throw new ConcurrencyException();
        }
    } 
}