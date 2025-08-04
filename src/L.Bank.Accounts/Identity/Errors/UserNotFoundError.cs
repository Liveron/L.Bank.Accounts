using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Errors;

namespace L.Bank.Accounts.Identity.Errors;

public record UserNotFoundError : NotFoundError
{
    public UserNotFoundError(Guid userId)
        : base($"Не удалось найти пользователя с ID {userId}.") { }
}

public static partial class MbResultFactoryExtensions
{
    public static MbResult<TResult> FailUserNotFound<TResult>(this IMbResultFactory factory, Guid userId)
    {
        var error = new UserNotFoundError(userId);
        return MbResult.Fail<TResult>(error);
    }

    public static MbResult FailUserNotFound(this IMbResultFactory factory, Guid userId)
    {
        var error = new UserNotFoundError(userId);
        return MbResult.Fail(error);
    }
}