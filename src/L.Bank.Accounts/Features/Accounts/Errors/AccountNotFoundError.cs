using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Errors;

namespace L.Bank.Accounts.Features.Accounts.Errors;

public record AccountNotFoundError : NotFoundError
{
    public AccountNotFoundError(Guid accountId)
        : base($"Не удалось найти счет с ID {accountId}.") { }
}

public static partial class MbResultFactoryExtensions
{
    public static MbResult<TResult> FailAccountNotFound<TResult>(
        this IMbResultFactory factory, Guid accountId)
    {
        var error = new AccountNotFoundError(accountId);
        return MbResult.Fail<TResult>(error);
    }

    public static MbResult FailAccountNotFound(
        this IMbResultFactory factory, Guid accountId)
    {
        var error = new AccountNotFoundError(accountId);
        return MbResult.Fail(error);
    }
}