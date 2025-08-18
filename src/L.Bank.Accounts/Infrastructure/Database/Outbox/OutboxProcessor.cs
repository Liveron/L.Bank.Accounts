using L.Bank.Accounts.Infrastructure.MassTransit;
using MassTransit;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public interface IOutboxProcessor
{
    Task ExecuteAsync();
}

public class OutboxProcessor(IPublishEndpoint publishEndpoint, IOutboxService outbox) : IOutboxProcessor
{
    public async Task ExecuteAsync()
    {
        var notPublishedEvents = await outbox.GetNotPublishedEvents();

        foreach (var @event in notPublishedEvents)
        {
            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await publishEndpoint.PublishIntegrationEvent(@event.IntegrationEvent!, source.Token);
            await outbox.MarkEventAsPublishedAsync(@event.Id);
        }
    }
}