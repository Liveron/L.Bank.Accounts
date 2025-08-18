using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public sealed record UnblockClientCommand : IRequest<MbResult>
{
    public required Guid ClientId { get; init; }
}