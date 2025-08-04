using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

public sealed record CloseAccountCommand : IRequest<MbResult>
{
    public required Guid AccountId { get; init; }
    public required Guid OwnerId { get; init; }
}