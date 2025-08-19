using L.Bank.Accounts.Features.Accounts;

namespace L.Bank.Accounts.UnitTests;

public sealed class AccountTests
{
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly Guid _ownerId = Guid.NewGuid();
    private const string Currency = "RUB";

    #region New Method Tests

    [Fact]
    public void New_WithValidParameters_ShouldCreateAccount()
    {
        // Arrange
        const decimal initialSum = 1000m;

        // Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Checking, Currency, sum: initialSum);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_accountId, result.Value!.Id);
        Assert.Equal(_ownerId, result.Value.OwnerId);
        Assert.Equal(AccountType.Checking, result.Value.Type);
        Assert.Equal(0m, result.Value.InterestRate);
        Assert.Equal(Currency, result.Value.Currency);
        Assert.Equal(initialSum, result.Value.Balance);
        Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), result.Value.OpenDate);
        Assert.Null(result.Value.CloseDate);
        Assert.Single(result.Value.Transactions);
    }

    [Fact]
    public void New_DepositAccountWithoutMaturityDate_ShouldFail()
    {
        // Arrange & Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Reliable6, Currency);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void New_WithMaturityDateInPast_ShouldFail()
    {
        // Arrange
        var pastDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        // Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Reliable6, Currency, pastDate);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void New_WithNegativeInitialSum_ShouldFail()
    {
        // Arrange & Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Checking, Currency, sum: -100m);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void New_DepositAccountWithValidMaturityDate_ShouldCreateAccount()
    {
        // Arrange
        var futureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6));

        // Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Reliable6, Currency, futureDate);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AccountType.Deposit, result.Value!.Type);
        Assert.Equal(futureDate, result.Value.MaturityDate);
    }

    [Fact]
    public void New_WithZeroInitialSum_ShouldCreateAccountWithZeroBalance()
    {
        // Arrange & Act
        var result = Account.New(_accountId, _ownerId, AccountTerms.Checking, Currency, sum: 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value!.Balance);
        Assert.Empty(result.Value.Transactions);
    }

    #endregion
}