using L.Bank.Accounts.Application;
using L.Bank.Accounts.Infrastructure.Integration.Outbox;
using L.EventBus.Abstractions;

namespace L.Bank.Accounts.Infrastructure.Integration;

public sealed class IntegrationService(ILogger<IntegrationService> logger, 
    IOutboxService outbox, IEventBus eventBus) 
    : IIntegrationService<IntegrationEvent>
{
    public async Task AddAndSaveAsync(IntegrationEvent @event)
    {
        logger.LogInformation("Adding integration event to outbox: {IntegrationEvent}", @event);

        await outbox.SaveEventAsync(@event);
    }

    public async Task PublishEventsAsync()
    {
        var notPublishedEvents = await outbox.GetNotPublishedEventsAsync();

        foreach (var @event in notPublishedEvents)
        {
            logger.LogInformation("Publishing integration event from outbox: {IntegrationEvent}", @event);

            try
            {
                await eventBus.PublishAsync(@event.IntegrationEvent!);
                await outbox.MarkEventAsPublishedAsync(@event.Id);
            }
            catch (TimeoutException)
            {
                logger.LogError("Integration event was not published. Event={IntegrationEvent}", @event.IntegrationEvent);
            }
        }
    }
}
