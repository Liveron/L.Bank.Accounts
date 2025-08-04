using System.Reflection;
using MediatR;

namespace L.Bank.Accounts.Common;

public class MbResult
{
    public bool IsSuccess => Error is null;
    public MbError? Error { get; private set; }
    public bool IsFailure => Error is not null;

    protected MbResult(MbError? error = null)
    {
        Error = error;
    }

    public static MbResult Fail(string message) => new(new MbError([message]));
    public static MbResult Fail(IEnumerable<string> messages) => new(new MbError(messages));
    public static MbResult Fail(MbError error) => new(error);

    public static MbResult Fail(Type resultType, MbError error)
    {
        var mbResultType = typeof(MbResult<>).MakeGenericType(resultType);
        var constructor = mbResultType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance, null, [resultType, typeof(MbError)], null);

        var defaultValue = resultType.IsValueType ? Activator.CreateInstance(resultType) : null;

        return (MbResult)constructor!.Invoke([defaultValue, error]);
    }

    public static MbResult<TResult> Fail<TResult>(string message) => new(default, new MbError([message]));
    public static MbResult<TResult> Fail<TResult>(IEnumerable<string> messages) => new(default, new MbError(messages));
    public static MbResult<TResult> Fail<TResult>(MbError error) => new(default, error);
    public static MbResult Success() => new();
    public static MbResult<TResult> Success<TResult>(TResult value) => new(value);
}

public sealed class MbResult<TResult> : MbResult, IRequest
{
    public TResult? Value { get; private set; }

    internal MbResult(TResult? value, MbError? error = null) 
        : base(error) => Value = value; 
}

public record MbError(IEnumerable<string> Messages);
