using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace L.Bank.Accounts.Infrastructure.Database.Inbox;

public sealed class InboxConsumeEventEntryEntityTypeConfiguration
    : IEntityTypeConfiguration<InboxConsumeEventEntry>
{
    public void Configure(EntityTypeBuilder<InboxConsumeEventEntry> builder)
    {
        builder.ToTable("inbox_consumed");

        builder.HasKey(x => x.MessageId);
    }
}
