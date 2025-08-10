using L.Bank.Accounts.Features.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Database.EntityConfigurations;

public sealed class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasIndex(a => a.OwnerId)
            .HasMethod("hash");

        builder.Property(a => a.Id);
        builder.Property(a => a.Type);
        builder.Property(a => a.OpenDate);

        builder.HasMany(a => a.Transactions)
            .WithOne();
    }
}