using System.Data.Common;
using L.Bank.Accounts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace L.Bank.Accounts.Features.Accounts.AccrueInterest;

public sealed record AccrueInterestCommand(Guid AccountId) : IRequest<MbResult>;