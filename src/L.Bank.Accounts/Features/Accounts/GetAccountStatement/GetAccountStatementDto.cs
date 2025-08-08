using JetBrains.Annotations;
using Mapster;

namespace L.Bank.Accounts.Features.Accounts.GetAccountStatement;

/// <summary>
/// DTO для запроса выписки по счету за указанный период
/// </summary>
public sealed record GetAccountStatementDto
{
    /// <summary>
    /// Начальная дата периода, за который запрашивается выписка
    /// </summary>
    [UsedImplicitly]
    public required DateOnly StartDate { get; init; }
    /// <summary>
    /// Конечная дата периода, за который запрашивается выписка
    /// </summary>
    [UsedImplicitly]
    public required DateOnly EndDate { get; init; }
}

public static class MappingExtensions
{
    static MappingExtensions()
    {
        TypeAdapterConfig<GetAccountStatementDto, GetAccountStatementQuery>.NewConfig()
            .Map(q => q.AccountId, 
            _ => (Guid)MapContext.Current!.Parameters["AccountId"]);
    }

    public static GetAccountStatementQuery MapToGetAccountStatementQuery(this GetAccountStatementDto dto, Guid accountId)
    {
        return dto.BuildAdapter()
            .AddParameters("AccountId", accountId)
            .AdaptToType<GetAccountStatementQuery>();
    }
}