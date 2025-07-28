using L.Bank.Accounts.Features.Accounts.CreateTransaction;
using Mapster;

namespace L.Bank.Accounts.Features.Accounts;

public sealed record CreateTransactionDto(
    Guid? CounterpartyAccountId,
    decimal Sum,
    TransactionType TransactionType,
    string? Description);

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig<CreateTransactionDto, CreateTransactionCommand>.NewConfig()
            .Map(command => command.AccountId, _ => (Guid)MapContext.Current!.Parameters["AccountId"]);
    }

    public static CreateTransactionCommand AdaptToCreateTransactionCommand(this CreateTransactionDto dto, Guid accountId)
    {
        return dto.BuildAdapter()
            .AddParameters("AccountId", accountId)
            .AdaptToType<CreateTransactionCommand>();
    }
}