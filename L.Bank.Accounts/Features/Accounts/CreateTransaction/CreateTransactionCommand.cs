using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

public sealed record CreateTransactionCommand(
    Guid AccountId, Guid? CounterpartyAccountId, decimal Sum, TransactionType TransactionType, string? Description) 
    : IRequest;