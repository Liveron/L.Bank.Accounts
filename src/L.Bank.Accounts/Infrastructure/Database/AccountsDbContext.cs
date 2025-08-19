using System.Data;
using L.Bank.Accounts.Features.Accounts;
using L.Bank.Accounts.Features.Accounts.EntityConfigurations;
using L.Bank.Accounts.Infrastructure.Database.Inbox;
using L.Bank.Accounts.Infrastructure.Database.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace L.Bank.Accounts.Infrastructure.Database;

public sealed class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : DbContext(options)
{
    public IDbContextTransaction? CurrentTransaction { get; private set; }
    public bool HasActiveTransaction => CurrentTransaction != null;

    public DbSet<Account> Accounts { get; set; }
    public DbSet<OutboxEventEntry> EventEntries { get; set; }
    public DbSet<InboxConsumeEventEntry> InboxConsumeEventEntries { get; set; }
    public DbSet<InboxDeadEventEntry> InboxDeadEventEntries { get; set; }

    public async Task<IDbContextTransaction?> BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        if (CurrentTransaction != null)
            return null;

        CurrentTransaction = await Database.BeginTransactionAsync(isolationLevel);

        return CurrentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction != CurrentTransaction) 
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                CurrentTransaction.Dispose();
                CurrentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            CurrentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                CurrentTransaction!.Dispose();
                CurrentTransaction = null;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxConsumeEventEntryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InboxDeadEventEntryEntityTypeConfiguration());
    }
}