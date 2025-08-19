using L.Bank.Accounts.Infrastructure.Database;
using L.Bank.Accounts.Infrastructure.Database.Inbox;
using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public sealed class IdempotencyFilter<T>(AccountsDbContext dbContext) 
    : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var consumedEvent = dbContext.InboxConsumeEventEntries
            .FirstOrDefault(x => x.MessageId == context.MessageId);

        if (consumedEvent != null)
            return;

        await next.Send(context);

        var newEvent = new InboxConsumeEventEntry(context.MessageId!.Value, DateTime.UtcNow);
        dbContext.InboxConsumeEventEntries.Add(newEvent);
        await dbContext.SaveChangesAsync();
    }

    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }
}