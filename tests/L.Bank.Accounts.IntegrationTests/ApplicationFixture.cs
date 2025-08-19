using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace L.Bank.Accounts.IntegrationTests;

[CollectionDefinition("Application Test Collection")]
public sealed class ApplicationCollectionFixture : ICollectionFixture<ApplicationFixture>;

public sealed class ApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.5-alpine3.22")
        .WithPortBinding(5432, 5432)
        .Build();

    public RabbitMqContainer RabbitMqContainer { get; } = new RabbitMqBuilder()
        .WithImage("rabbitmq:4.1.3-management-alpine")
        .WithResourceMapping(new FileInfo("rabbitmq.conf"), new FileInfo("/etc/rabbitmq/rabbitmq.conf"))
        .WithResourceMapping(new FileInfo("rabbitmq-definitions.json"), new FileInfo("/etc/rabbitmq/definitions.json"))
        .WithPortBinding(5672, 5672)
        .WithPortBinding(15672, 15672)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAssertion(_ => true)
                    .Build());
        });
    }

    public async Task InitializeAsync()
    {
        await Task.WhenAll(_dbContainer.StartAsync(), RabbitMqContainer.StartAsync());
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Task.WhenAll(_dbContainer.StopAsync(), RabbitMqContainer.StopAsync());
        await _dbContainer.DisposeAsync();
        await RabbitMqContainer.DisposeAsync();
    }
}