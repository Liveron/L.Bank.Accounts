using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Errors;

namespace L.Bank.Accounts.Features.Accounts.Errors;

public sealed record PropertyNotFoundError : NotFoundError
{
    public PropertyNotFoundError(string propertyName)
        : base($"Property {propertyName} is not found.") { }
}

public static partial class MbResultFactoryExtensions
{
    public static MbResult<TResult> FailPropertyNotFound<TResult>(
        this IMbResultFactory factory, string property)
    {
        var error = new PropertyNotFoundError(property);
        return MbResult.Fail<TResult>(error);
    }
}