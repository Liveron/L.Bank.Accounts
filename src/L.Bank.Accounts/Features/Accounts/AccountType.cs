using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Features.Accounts;

/// <summary>
/// Тип счета
/// </summary>
public enum AccountType
{
    [JsonStringEnumMemberName("Текущий")]
    Checking,
    [JsonStringEnumMemberName("Депозит")]
    Deposit,
    [JsonStringEnumMemberName("Кредит")]
    // ReSharper disable once UnusedMember.Global Нужно по заданию
    Credit
}