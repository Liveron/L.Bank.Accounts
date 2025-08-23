namespace L.Bank.Accounts.Application;

public interface IIntegrationService<TEvent>
{
    public Task PublishEventsAsync();
    public Task AddAndSaveAsync(TEvent @event);
}
