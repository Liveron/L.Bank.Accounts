namespace L.Bank.Accounts.Infrastructure.Database.Inbox;

public sealed record InboxDeadEventEntry(
    Guid MessageId, DateTime ReceivedAt, string Handler, string Payload, string Error);