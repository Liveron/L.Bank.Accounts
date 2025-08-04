using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAccountProperty;

public sealed record GetAccountPropertyQuery : IRequest<MbResult<object?>>
{
    public required Guid OwnerId { get; init; }
    public required Guid AccountId { get; init; }
    public required string PropertyName { get; init; }
}