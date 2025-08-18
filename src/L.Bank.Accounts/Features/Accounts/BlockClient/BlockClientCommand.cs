using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed record BlockClientCommand(Guid ClientId) : IRequest<MbResult>;