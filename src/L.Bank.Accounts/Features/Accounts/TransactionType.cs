using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Features.Accounts;

public enum TransactionType
{
    [JsonStringEnumMemberName("Кредит")]
    Credit,
    [JsonStringEnumMemberName("Дебет")]
    Debit
}
