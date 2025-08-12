using System.Reflection;
using MediatR;

namespace L.Bank.Accounts.Common;

public class MbResult(MbError? error = null)
{
    /// <summary>
    /// Свойство, определяющее, успешно ли завершилась операция
    /// </summary>
    public bool IsSuccess => Error is null;
    /// <summary>
    /// Ошибка, возращаемая при неуспешном завершении операции
    /// </summary>
    public MbError? Error { get; } = error;

    /// <summary>
    /// Свойство, определяющее, завершилась ли операция с ошибкой
    /// </summary>
    public bool IsFailure => Error is not null;

    public static MbResult Fail(string message) => new(new MbError([message]));
    public static MbResult Fail(IEnumerable<string> messages) => new(new MbError(messages));
    public static MbResult Fail(MbError error) => new(error);
    public static MbResult Fail(Type resultType, string message) => Fail(resultType, new MbError([message]));
    public static MbResult Fail(Type resultType, MbError error)
    {
        var mbResultType = typeof(MbResult<>).MakeGenericType(resultType);
        var constructor = mbResultType.GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance, null, [resultType, typeof(MbError)], null);

        var defaultValue = resultType.IsValueType ? Activator.CreateInstance(resultType) : null;

        return (MbResult)constructor!.Invoke([defaultValue, error]);
    }

    public static MbResult<TResult> Fail<TResult>(string message) => new(default, new MbError([message]));
    public static MbResult<TResult> Fail<TResult>(MbError error) => new(default, error);
    public static MbResult Success() => new();
    public static MbResult<TResult> Success<TResult>(TResult value) => new(value);
}

public sealed class MbResult<TResult>(TResult? value, MbError? error = null) 
    : MbResult(error), IRequest
{
    /// <summary>
    /// Значение, возвращаемое при успешном завершении операции
    /// </summary>
    public TResult? Value { get; private set; } = value;
}

/// <summary>
/// Объект ошибки
/// </summary>
/// <param name="Messages">Коллекция сообщений, описывающих возникшие ошибки</param>
public record MbError(IEnumerable<string> Messages);
