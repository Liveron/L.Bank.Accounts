using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Features.Accounts;

public enum AccountType
{
    [JsonStringEnumMemberName("Текущий")]
    Checking,
    [JsonStringEnumMemberName("Депозит")]
    Deposit,
    [JsonStringEnumMemberName("Кредит")]
    Credit
}