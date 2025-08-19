using Mapster;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed record ChangeInterestRateDto
{
    /// <summary>
    /// ID владельца счета
    /// </summary>
    public required Guid OwnerId { get; init; }
    /// <summary>
    /// Процентная ставка
    /// </summary>
    public required decimal InterestRate { get; init; }
}

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig<ChangeInterestRateDto, ChangeInterestRateCommand>.NewConfig()
            .Map(query => query.AccountId,
                _ => (Guid)MapContext.Current!.Parameters["AccountId"]);
    }

    public static ChangeInterestRateCommand MapToChangeInterestRateCommand(
        this ChangeInterestRateDto dto, Guid accountId)
    {
        return dto.BuildAdapter()
            .AddParameters("AccountId", accountId)
            .AdaptToType<ChangeInterestRateCommand>();
    }
}