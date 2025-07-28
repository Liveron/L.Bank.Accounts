using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Features.Accounts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    [JsonStringEnumMemberName("Кредит")]
    Credit,
    [JsonStringEnumMemberName("Дебет")]
    Debit
}
