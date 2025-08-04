using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed record TransferCommand(
    Guid FromAccountId, 
    Guid ToAccountId, 
    Guid FromAccountOwnerId, 
    Guid ToAccountOwnerID, 
    decimal Sum, 
    string? Description) 
    : IRequest<MbResult>;