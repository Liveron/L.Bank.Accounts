using System.Data;
using System.Reflection;
using L.Bank.Accounts.Common.Attributes;
using L.Bank.Accounts.Common.Behaviors;
using MediatR;

namespace L.Bank.Accounts.Extensions;

public static class TransactionLevelHandlerMapExtensions
{
    public static void AddTransactionLevelHandlerMap(this IServiceCollection services)
    {
        var transactionLevelHandlerMap = CreateTransactionLevelHandlerMap();
        services.AddSingleton<ITransactionLevelHandlerMap>(transactionLevelHandlerMap);
    }

    private static TransactionLevelHandlerMap CreateTransactionLevelHandlerMap()
    {
        var transactionTypes = ScanAssemblyForTransactionalTypes();

        Dictionary<Type, IsolationLevel> transactionHandlerTypes = [];
        foreach (var type in transactionTypes)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                var genericDefinition = interfaceType.GetGenericTypeDefinition();
                var genericDefinitions = interfaceType.GetGenericArguments();

                if (genericDefinition != typeof(IRequestHandler<>) &&
                    genericDefinition != typeof(IRequestHandler<,>)) 
                    continue;

                if (transactionHandlerTypes.ContainsKey(genericDefinitions[0])) 
                    continue;

                var isolationLevel = type.GetCustomAttribute<TransactionAttribute>()!.IsolationLevel;
                transactionHandlerTypes[genericDefinitions[0]] = isolationLevel;
            }
        }

        return new TransactionLevelHandlerMap(transactionHandlerTypes);
    }



    private static List<Type> ScanAssemblyForTransactionalTypes()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false }
                           && type.GetCustomAttribute<TransactionAttribute>() != null)
            .ToList();
    }
}

