using L.Bank.Accounts.Common;

namespace L.Bank.Accounts.Extensions;

public static class MbResultExtensions
{
    public static bool IsMbResult(this Type type)
    {
        return type == typeof(MbResult);
    }

    public static bool IsGenericMbResult(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MbResult<>);
    }
}