using L.EventBus.DependencyInjection;
using L.EventBus.RabbitMQ.DependencyInjection.Configuration;
using RabbitMQ.Client;

namespace L.Bank.Accounts.Extensions.DependencyInjection;

public static class RabbitMqExtensions
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConnectionString = configuration.GetConnectionString("RabbitMQ");
        services.AddEventBus(config =>
        {
            config.UseRabbitMq(rabbitMqConnectionString! ,rmqConfig =>
            {
                rmqConfig.SetExchange(ExchangeType.Topic, "account.events", exchangeConfig =>
                {
                    exchangeConfig.SetQueue("account.crm", "account.*");
                    exchangeConfig.SetQueue("account.notifications", "account.money.*");
                    exchangeConfig.SetQueue("account.antifraud", "account.client.*");
                    exchangeConfig.SetQueue("account.audit", "#");
                });
            });
        });
    }
}