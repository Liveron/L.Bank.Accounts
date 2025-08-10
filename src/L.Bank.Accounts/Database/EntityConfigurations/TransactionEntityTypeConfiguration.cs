using L.Bank.Accounts.Features.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Database.EntityConfigurations;

public sealed class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasIndex(t => t.DateTime)
            .HasMethod("GIST");

        builder.HasIndex(t => new { t.AccountId, t.DateTime });
    }
}