using L.Bank.Accounts.Features.Accounts;
using L.Bank.Accounts.Features.Accounts.IntegrationEvents;
using L.Bank.Accounts.Infrastructure.Database.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace L.Bank.Accounts.IntegrationTests;

[Collection("Application Test Collection")]
public sealed class OutboxTests(ApplicationFixture fixture)
{
    [Fact]
    public async Task OutboxProcessor_Publishes_AfterFailure()
    {
        var integrationEvent = new AccountOpenedIntegrationEvent(
            Guid.NewGuid(), Guid.NewGuid(), "RUB", AccountType.Checking);
        await using var scope = fixture.Services.CreateAsyncScope();
        var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
        var outboxProcessor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

        await fixture.RabbitMqContainer.StopAsync();

        await outboxService.SaveEventAsync(integrationEvent);

        await outboxProcessor.ExecuteAsync();

        var events = await outboxService.GetNotPublishedEvents();

        Assert.NotEmpty(events);

        await fixture.RabbitMqContainer.StartAsync();

        await outboxProcessor.ExecuteAsync();

        events = await outboxService.GetNotPublishedEvents();
        Assert.Empty(events);
    }
}