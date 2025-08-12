using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.AccrueInterest;

public sealed record AccrueInterestCommand(Guid AccountId) : IRequest<MbResult>;