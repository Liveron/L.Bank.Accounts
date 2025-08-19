using L.Bank.Accounts.Infrastructure.MassTransit;
using MassTransit;

namespace L.Bank.Accounts.Infrastructure.Database.Outbox;

public interface IOutboxProcessor
{
    Task ExecuteAsync();
}

public class OutboxProcessor(
    IPublishEndpoint publishEndpoint, IOutboxService outbox, ILogger<OutboxProcessor> logger) 
    : IOutboxProcessor
{
    public async Task ExecuteAsync()
    {
        var notPublishedEvents = await outbox.GetNotPublishedEvents();

        foreach (var @event in notPublishedEvents)
        {
            var integrationEvent = @event.IntegrationEvent;
            try
            {
                await publishEndpoint.PublishIntegrationEvent(integrationEvent!);
                await outbox.MarkEventAsPublishedAsync(@event.Id);
            }
            catch (TimeoutException)
            {
                logger.LogError("Integration event was not published. Event={IntegrationEvent}", integrationEvent);
            }
        }
    }
}