using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid AccountId, Guid OwnerId, Guid? CounterpartyAccountId, decimal Sum, TransactionType TransactionType, string? Description) 
    : IRequest<MbResult>;