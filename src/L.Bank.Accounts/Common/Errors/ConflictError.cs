namespace L.Bank.Accounts.Common.Errors;

public record ConflictError : MbError
{
    public ConflictError(string message) : base([message]) { }
}