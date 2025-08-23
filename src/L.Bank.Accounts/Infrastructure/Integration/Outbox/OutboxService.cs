using System.Text.Json;
using L.Bank.Accounts.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace L.Bank.Accounts.Infrastructure.Integration.Outbox;

public interface IOutboxService
{
    Task SaveEventAsync(IntegrationEvent @event);
    Task<IEnumerable<OutboxEventEntry>> GetNotPublishedEventsAsync();
    Task MarkEventAsPublishedAsync(Guid eventId);
}

public sealed class OutboxService(AccountsDbContext dbContext) : IOutboxService
{
    public async Task SaveEventAsync(IntegrationEvent @event)
    {
        dbContext.EventEntries.Add(new OutboxEventEntry
        {
            Event = JsonSerializer.Serialize(@event),
            EventType = @event.GetType().AssemblyQualifiedName!
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<OutboxEventEntry>> GetNotPublishedEventsAsync()
    {
        var entries = await dbContext.EventEntries.Where(e => e.Status == OutboxEventEntryStatus.NotPublished)
            .OrderBy(e => e.SavedAt)
            .ToListAsync();

        if (entries.Count != 0)
        {
            return entries.Select(e => e.DeserializeJsonEvent(Type.GetType(e.EventType)!));
        }

        return [];
    }

    public async Task MarkEventAsPublishedAsync(Guid eventId)
    {
        var eventEntry = dbContext.EventEntries.FirstOrDefault(e => e.Id == eventId);

        if (eventEntry != null)
        {
            eventEntry.Status = OutboxEventEntryStatus.Published;
            await dbContext.SaveChangesAsync();
        }
    }
}