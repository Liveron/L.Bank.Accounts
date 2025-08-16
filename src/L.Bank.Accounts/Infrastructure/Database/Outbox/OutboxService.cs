using System.Text.Json;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public interface IOutboxService
{
    Task SaveEventAsync(IntegrationEvent @event);
}

public sealed class OutboxService(AccountsDbContext dbContext) : IOutboxService
{
    public async Task SaveEventAsync(IntegrationEvent @event)
    {
        dbContext.EventEntries.Add(new OutboxEventEntry
        {
            Event = JsonSerializer.Serialize(@event),
            EventType = @event.GetType().AssemblyQualifiedName!,
        });

        await dbContext.SaveChangesAsync();
    }
}