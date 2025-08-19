namespace L.Bank.Accounts.Infrastructure.Database.Inbox;

public sealed record InboxConsumeEventEntry(Guid MessageId, DateTime ProcessedAt);