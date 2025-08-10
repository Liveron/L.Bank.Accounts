using L.Bank.Accounts.Database.EntityConfigurations;
using L.Bank.Accounts.Features.Accounts;
using Microsoft.EntityFrameworkCore;

namespace L.Bank.Accounts.Database;

public sealed class AccountsDbContext(DbContextOptions<AccountsDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => null!;
    public DbSet<Transaction> Transactions => null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
    }
}