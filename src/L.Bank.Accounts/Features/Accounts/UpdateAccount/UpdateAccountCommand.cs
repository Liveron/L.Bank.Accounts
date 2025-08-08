using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.UpdateAccount;

public sealed record UpdateAccountCommand : IRequest<MbResult>
{
    public required Guid OwnerId { get; init; }
    public required Guid AccountId { get; init; }
    public decimal? InterestRate { get; init; }
    public DateOnly? MaturityDate { get; init; }
}
