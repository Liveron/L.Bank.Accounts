using Mapster;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed record CreateTransactionDto
{
    /// <summary>
    /// Сумма транзакции
    /// </summary>
    /// <example>100</example>
    public required decimal Sum { get; init; }
    /// <summary>
    /// Тип транзакции
    /// </summary>
    /// <example>Кредит</example>
    public required TransactionType TransactionType { get; init; }
    /// <summary>
    /// ID контрагента
    /// </summary>
    public required Guid? CounterpartyAccountId { get; init; }
    /// <summary>
    /// Описание транзакции
    /// </summary>
    /// <example>Описание</example>
    public required string? Description { get; init; }
}

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig<CreateTransactionDto, CreateTransactionCommand>.NewConfig()
            .Map(command => command.AccountId, _ => (Guid)MapContext.Current!.Parameters["AccountId"]);
    }

    public static CreateTransactionCommand MapToCreateTransactionCommand(this CreateTransactionDto dto, Guid accountId)
    {
        return dto.BuildAdapter()
            .AddParameters("AccountId", accountId)
            .AdaptToType<CreateTransactionCommand>();
    }
}