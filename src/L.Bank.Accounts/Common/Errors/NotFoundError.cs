namespace L.Bank.Accounts.Common.Errors;

public record NotFoundError : MbError
{
    public NotFoundError(string message) : base([message]) { }
}