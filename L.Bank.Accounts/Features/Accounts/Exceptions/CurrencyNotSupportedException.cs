using L.Bank.Accounts.Common.Exceptions;

namespace L.Bank.Accounts.Features.Accounts.Exceptions;

public class CurrencyNotSupportedException(string currencyCode)
    : DomainException($"Валюта с кодом \"{currencyCode}\" не поддерживается.");