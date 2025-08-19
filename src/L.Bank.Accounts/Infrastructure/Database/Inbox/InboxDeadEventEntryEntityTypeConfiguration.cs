using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Infrastructure.Database.Inbox;

public sealed class InboxDeadEventEntryEntityTypeConfiguration : IEntityTypeConfiguration<InboxDeadEventEntry>
{
    public void Configure(EntityTypeBuilder<InboxDeadEventEntry> builder)
    {
        builder.ToTable("InboxDeadEventEntries", "inbox_dead_letters");

        builder.HasKey(x => x.MessageId);
    }
}
