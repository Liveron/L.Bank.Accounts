using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

public sealed record CheckAccountExistsQuery : IRequest<MbResult<bool>>
{
    public required Guid OwnerId { get; init; }
    public required AccountType? AccountType { get; init; }
}