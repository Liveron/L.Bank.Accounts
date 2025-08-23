using System.Net;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts;
using L.Bank.Accounts.Features.Accounts.OpenAccount;
using L.Bank.Accounts.Features.Accounts.Transfer;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using L.Bank.Accounts.Features.Accounts.BlockClient;
using L.Bank.Accounts.Features.Accounts.ChangeInterestRate;
using L.Bank.Accounts.Features.Accounts.CreateTransaction;

namespace L.Bank.Accounts.IntegrationTests;

[Collection("Application Test Collection")]
public sealed class ApiTests(ApplicationFixture fixture)
{
    private readonly HttpClient _client = fixture.CreateDefaultClient();

    private async Task<Guid> OpenAccountAsync(Guid ownerId, AccountTerms accountTerms)
    {
        var command = new OpenAccountCommand
        {
            OwnerId = ownerId,
            AccountTerms = accountTerms.Name,
            Currency = "RUB",
            Sum = 1000,
            MaturityDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30))
        };
        var response = await _client.PostAsJsonAsync("api/accounts", command);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MbResult<Guid>>();
        return result!.Value;
    }

    private async Task<HttpResponseMessage> RegisterDebitTransaction(Guid ownerId, Guid accountId, decimal amount)
    {
        var dto = new CreateTransactionDto
        {
            OwnerId = ownerId,
            Sum = amount,
            TransactionType = TransactionType.Debit
        };
        return await _client.PostAsJsonAsync($"api/accounts/{accountId}/transactions", dto);
    }

    [Fact]
    public async Task Transfer_WhenManyRequests_ShouldSaveBalance()
    {
        // Arrange
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var firstAccountId = await OpenAccountAsync(firstUserId, AccountTerms.Checking);
        var secondAccountId = await OpenAccountAsync(secondUserId, AccountTerms.Checking);

        var command = new TransferCommand
        {
            FromAccountId = firstAccountId,
            ToAccountId = secondAccountId,
            FromAccountOwnerId = firstUserId,
            ToAccountOwnerId = secondUserId,
            Sum = 500
        };

        // Act
        decimal sumBeforeTransfers;
        await using (var scope = fixture.Services.CreateAsyncScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
            var firstAccount = await repository.GetAccountAsync(firstAccountId, firstUserId);
            var secondAccount = await repository.GetAccountAsync(secondAccountId, secondUserId);
            sumBeforeTransfers = firstAccount!.Balance + secondAccount!.Balance;
        }

        var tasks = Enumerable.Range(0, 50)
            .Select(_ => Task.Run(() => _client.PostAsJsonAsync("api/accounts/transfer", command)))
            .ToArray();

        await Task.WhenAll(tasks);

        decimal sumAfterTransfers;
        await using (var scope = fixture.Services.CreateAsyncScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
            var firstAccount = await repository.GetAccountAsync(firstAccountId, firstUserId);
            var secondAccount = await repository.GetAccountAsync(secondAccountId, secondUserId);
            sumAfterTransfers = firstAccount!.Balance + secondAccount!.Balance;
        }

        Assert.Equal(sumBeforeTransfers, sumAfterTransfers);
    }

    [Fact]
    public async Task ChangeAccount_WhenConcurrent_ShouldReturn409()
    {
        // Arrange & Act
        var ownerId = Guid.NewGuid();
        var accountId = await OpenAccountAsync(ownerId, AccountTerms.Reliable6);

        var dto = new ChangeInterestRateDto
        {
            InterestRate = 10m,
            OwnerId = ownerId
        };

        var firstTask = _client.PutAsJsonAsync($"api/accounts/{accountId}/interest-rate", dto);
        var secondTask = _client.PutAsJsonAsync($"api/accounts/{accountId}/interest-rate", dto);

        await Task.WhenAll(firstTask, secondTask);

#pragma warning disable xUnit1031
        var firstResponse = firstTask.Result;
        var secondResponse = secondTask.Result;
#pragma warning restore xUnit1031

        Assert.NotEqual(firstResponse.StatusCode, secondResponse.StatusCode);
        var expectedValues = new[] { HttpStatusCode.OK, HttpStatusCode.Conflict };
        Assert.Contains(firstResponse.StatusCode, expectedValues);
        Assert.Contains(secondResponse.StatusCode, expectedValues);
    }

    //[Fact]
    //public async Task ClientBlocked_ShouldPreventDebit()
    //{
    //    // Arrange & Act
    //    var ownerId = Guid.NewGuid();
    //    var accountId = await OpenAccountAsync(ownerId, AccountTerms.Reliable6);

    //    var @event = new ClientBlockedIntegrationEvent(ownerId);
    //    await using var scope = fixture.Services.CreateAsyncScope();

    //    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
    //    await publishEndpoint.PublishIntegrationEvent(@event);

    //    await Task.Delay(10000); // Жду обработки события

    //    var response = await RegisterDebitTransaction(ownerId, accountId, 100);

    //    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    //}
}