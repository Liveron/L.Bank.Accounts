using System.Text.Json.Serialization;

namespace L.Bank.Accounts.Features.Accounts;

/// <summary>
/// Тип транзакции
/// </summary>
public enum TransactionType
{
    [JsonStringEnumMemberName("Кредит")]
    Credit,
    [JsonStringEnumMemberName("Дебет")]
    Debit
}
