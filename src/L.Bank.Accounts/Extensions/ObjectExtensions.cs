using System.Reflection;

namespace L.Bank.Accounts.Extensions;

public static class ObjectExtensions
{
    public static bool TryGetPropertyValue(this object obj, string propertyName, out object? propertyValue)
    {
        propertyValue = null;

        var property = obj.GetType().GetProperty(propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property is null)
            return false;

        propertyValue = property.GetValue(obj);
        return true;
    }
}