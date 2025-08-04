using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed record ChangeInterestRateCommand(Guid AccountId, Guid OwnerId, decimal InterestRate) 
    : IRequest<MbResult>;