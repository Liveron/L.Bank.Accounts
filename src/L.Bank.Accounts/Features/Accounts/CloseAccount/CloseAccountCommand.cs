using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

public sealed record CloseAccountCommand(Guid AccountId, Guid OwnerId) : IRequest<MbResult>
{
    public Guid AccountId { get; } = AccountId;
}