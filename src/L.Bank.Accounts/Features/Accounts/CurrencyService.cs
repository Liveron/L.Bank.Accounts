using L.Bank.Accounts.Common;

namespace L.Bank.Accounts.Features.Accounts;

public interface ICurrencyService
{
    public bool CheckCurrency(string currencyCode);
}

public sealed class CurrencyService : ICurrencyService
{
    private static readonly string[] ValidCurrencyCodes = [ "RUB" ];

    public bool CheckCurrency(string currencyCode)
    {
        return ValidCurrencyCodes.Contains(currencyCode);
    }
}
