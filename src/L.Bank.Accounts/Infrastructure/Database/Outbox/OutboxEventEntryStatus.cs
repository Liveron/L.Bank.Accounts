namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public enum OutboxEventEntryStatus
{
    NotPublished = 0,
    Published = 1
}