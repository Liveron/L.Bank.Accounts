using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Features.Accounts.EntityConfigurations;

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
        builder.Property(a => a.Currency)
            .HasMaxLength(3);

        //builder.Metadata.FindNavigation(nameof(Account.Transactions))!
        //    .SetField("_transactions");

        builder.Property(a => a.Xmin)
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();

        builder.HasMany(a => a.Transactions)
            .WithOne();
    }
}