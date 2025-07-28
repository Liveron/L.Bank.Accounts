using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed record TransferCommand(Guid FromAccountId, Guid ToAccountId, decimal Sum, string? Description) : IRequest;