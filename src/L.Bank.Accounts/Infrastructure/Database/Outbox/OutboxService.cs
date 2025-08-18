using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public interface IOutboxService
{
    Task SaveEventAsync(IntegrationEvent @event);
    Task<IEnumerable<OutboxEventEntry>> GetNotPublishedEvents();
    Task MarkEventAsPublishedAsync(Guid eventId);
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

    public async Task<IEnumerable<OutboxEventEntry>> GetNotPublishedEvents()
    {
        var entries = await dbContext.EventEntries.Where(e => e.Status == OutboxEventEntryStatus.NotPublished)
            .OrderBy(e => e.SavedAt)
            .ToListAsync();

        return entries.Count != 0 
            ? entries.Select(e => e.DeserializeJsonEvent(Type.GetType(e.EventType)!)) 
            : [];
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