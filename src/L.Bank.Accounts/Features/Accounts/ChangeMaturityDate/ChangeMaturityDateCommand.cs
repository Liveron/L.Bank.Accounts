using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.ChangeMaturityDate;

public sealed record ChangeMaturityDateCommand(Guid AccountId, Guid OwnerId, DateOnly MaturityDate) 
    : IRequest<MbResult>;