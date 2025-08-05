using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed record ChangeInterestRateCommand : IRequest<MbResult>
{
    public required Guid AccountId { get; init; }
    public required Guid OwnerId { get; init; }
    public required decimal InterestRate { get; init; }
}