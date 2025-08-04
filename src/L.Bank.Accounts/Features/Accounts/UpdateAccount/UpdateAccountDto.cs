using L.Bank.Accounts.Features.Accounts.CreateTransaction;
using Mapster;

namespace L.Bank.Accounts.Features.Accounts.UpdateAccount;

public sealed record UpdateAccountDto(Guid OwnerId, decimal? InterestRate, DateOnly? MaturityDate)
{
    public Guid OwnerId { get; } = OwnerId;
    public decimal? InterestRate { get; } = InterestRate;
    public DateOnly? MaturityDate { get; } = MaturityDate;
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