using L.Bank.Accounts.Features.Accounts.CreateTransaction;
using Mapster;

namespace L.Bank.Accounts.Features.Accounts.UpdateAccount;

public sealed record UpdateAccountDto
{
    /// <summary>
    /// ID владельца счета
    /// </summary>
    public required Guid OwnerId { get; init; }

    /// <summary>
    /// Процентная ставка
    /// </summary>
    /// <example>10</example>
    public decimal? InterestRate { get; init; }

    /// <summary>
    /// Дата погашения счета (для срочных вкладов)
    /// </summary>
    public DateOnly? MaturityDate { get; init; }
}

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig<UpdateAccountDto, UpdateAccountCommand>.NewConfig()
            .Map(command => command.AccountId,
                _ => (Guid)MapContext.Current!.Parameters["AccountId"]);
    }

    public static CreateTransactionCommand MapToUpdateAccountCommand(this UpdateAccountDto dto, Guid accountId)
    {
        return dto.BuildAdapter()
            .AddParameters("AccountId", accountId)
            .AdaptToType<CreateTransactionCommand>();
    }
}