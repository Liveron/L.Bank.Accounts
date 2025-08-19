using System.Text.Json;
using L.Bank.Accounts.Infrastructure.Database;
using L.Bank.Accounts.Infrastructure.Database.Inbox;
using MassTransit;

namespace L.Bank.Accounts.Infrastructure.MassTransit;

public sealed class ValidationEnvelopeFilter<T>(
    ILogger<ValidationEnvelopeFilter<T>> logger, AccountsDbContext dbContext) 
    : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var version = context.Headers.Get<string>("Version");
        if (version is not "v1")
        {
            logger.LogWarning(
                "[ValidationEnvelopeFilter] Invalid version {Version} for message {MessageId}. Expected 'v1'.",
                version, context.MessageId);

            var entry = new InboxDeadEventEntry(
                context.MessageId!.Value,
                DateTime.UtcNow,
                string.Empty,
                JsonSerializer.Serialize(context.Message),
                string.Empty);
            dbContext.InboxDeadEventEntries.Add(entry);
            await dbContext.SaveChangesAsync();
            return;
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }
}