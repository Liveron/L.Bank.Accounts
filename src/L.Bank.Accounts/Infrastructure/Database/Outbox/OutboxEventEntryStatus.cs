namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public enum OutboxEventEntryStatus
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
}