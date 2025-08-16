using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public sealed class OutboxEventEntryEntityTypeConfiguration : IEntityTypeConfiguration<OutboxEventEntry>
{
    public void Configure(EntityTypeBuilder<OutboxEventEntry> builder)
    {
    }
}