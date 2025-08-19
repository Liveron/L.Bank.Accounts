using System.Data;
using L.Bank.Accounts.Infrastructure.Database;
using MediatR;

namespace L.Bank.Accounts.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse>(
    ITransactionLevelHandlerMap transactionLevelHandlerMap, AccountsDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isolationLevel = transactionLevelHandlerMap.GetTransactionLevel(request.GetType());
            
        if (dbContext.HasActiveTransaction)
            return await next(cancellationToken);

        await using var transaction = await dbContext.BeginTransactionAsync(isolationLevel);

        var result = await next(cancellationToken);

        await dbContext.CommitTransactionAsync(transaction!);

        return result;
    }
}

public interface ITransactionLevelHandlerMap
{
    public IsolationLevel GetTransactionLevel(Type requestType);
}

public sealed class TransactionLevelHandlerMap(Dictionary<Type, IsolationLevel> dictionary) : ITransactionLevelHandlerMap
{
    private readonly IReadOnlyDictionary<Type, IsolationLevel> _dictionary = dictionary.AsReadOnly();

    public IsolationLevel GetTransactionLevel(Type requestType)
    {
        return _dictionary.GetValueOrDefault(requestType, IsolationLevel.ReadCommitted);
    }
}