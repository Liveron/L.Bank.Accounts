namespace L.Bank.Accounts.Extensions;

public static class TaskExtensions
{
    public static bool IsGenericTask(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
    }
}