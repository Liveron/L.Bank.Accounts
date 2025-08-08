using L.Bank.Accounts.Common;

namespace L.Bank.Accounts.Features.Accounts.Errors;

public record CurrencyNotSupportedError : MbError
{
    public CurrencyNotSupportedError(string currencyCode)
        : base([ $"Валюта {currencyCode} не поддерживается." ]) { }
}

public static partial class MbResultFactoryExtensions
{
    public static MbResult<TResult> FailCurrencyNotSupported<TResult>(
        this IMbResultFactory factory, string currencyCode)
    {
        var error = new CurrencyNotSupportedError(currencyCode);
        return MbResult.Fail<TResult>(error);
    }
}