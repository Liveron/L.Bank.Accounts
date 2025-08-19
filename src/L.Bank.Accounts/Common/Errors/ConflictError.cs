namespace L.Bank.Accounts.Common.Errors;

public record ConflictError : MbError
{
    public ConflictError(string message) : base([message]) { }
}

public static class MbResultFactoryExtensions
{
    public static MbResult FailConflict(this IMbResultFactory factory, string errorMessage)
    {
        var error = new ConflictError(errorMessage);
        return MbResult.Fail(error);
    }
}