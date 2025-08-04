using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed record TransferCommand : IRequest<MbResult>
{
    /// <summary>
    /// ID аккаунта, с которого списываются деньги
    /// </summary>
    public required Guid FromAccountId { get; init; }
    /// <summary>
    /// ID аккаунта, на который переводятся деньги
    /// </summary>
    public required Guid ToAccountId { get; init; } 
    /// <summary>
    /// ID владельца аккаунта, с которого списываются деньги
    /// </summary>
    public required Guid FromAccountOwnerId { get; init; } 
    /// <summary>
    /// ID владельца аккаунта, на который переводятся деньги
    /// </summary>
    public required Guid ToAccountOwnerId { get; init; }
    /// <summary>
    /// Сумма перевода
    /// </summary>
    /// <example>100</example>
    public required decimal Sum { get; init; }
    /// <summary>
    /// Описание перевода
    /// </summary>
    /// <example>Описание</example>
    public string? Description { get; init; }
}