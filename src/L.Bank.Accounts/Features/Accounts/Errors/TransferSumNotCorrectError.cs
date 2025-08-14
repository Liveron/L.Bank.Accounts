using L.Bank.Accounts.Common;
using L.Bank.Accounts.Common.Errors;

namespace L.Bank.Accounts.Features.Accounts.Errors;

public sealed record TransferSumNotCorrectError : ConflictError
{
    public TransferSumNotCorrectError(decimal expectedSum, decimal actualSum)
        : base($"Сумма счетов после перевода средств не соответствует ожидаемой. " +
               $"Текущая сумма: {actualSum}. Ожидаемая сумма: {expectedSum}") { }
}

public static partial class MbResultFactoryExtensions
{
    public static MbResult FailTransferSumNotCorrect(
        this IMbResultFactory factory, decimal expectedSum, decimal actualSum)
    {
        var error = new TransferSumNotCorrectError(expectedSum, actualSum);
        return MbResult.Fail(error);
    }
}