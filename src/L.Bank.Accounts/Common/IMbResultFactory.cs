namespace L.Bank.Accounts.Common;

public interface IMbResultFactory
{
    MbResult Success();
    MbResult<TResult> Success<TResult>(TResult value);
    MbResult<TResult> Fail<TResult>(MbError error);
}

public sealed class MbResultFactory : IMbResultFactory
{
    public MbResult Success() => MbResult.Success();
    public MbResult<TResult> Success<TResult>(TResult value) => MbResult.Success(value);
    public MbResult<TResult> Fail<TResult>(MbError error) => MbResult.Fail<TResult>(error);
}