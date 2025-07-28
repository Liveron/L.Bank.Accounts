using System.Reflection;

namespace L.Bank.Accounts.Common;

public abstract class Enumeration<T> : IComparable
    where T : Enumeration<T>, new()
{
    protected Enumeration() { }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; } = null!;
    public int Id { get; }
    public int CompareTo(object? obj) => Id.CompareTo(((Enumeration<T>)obj!).Id);
    public static IEnumerable<T> GetAll()
    {
        var type = typeof(T);
        var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var field in fields)
        {
            var instance = new T();

            if (field.GetValue(instance) is T locatedValue)
            {
                yield return locatedValue;
            }
        }
    }

    public static T FromName(string name)
    {
        var matchingItem = GetAll().FirstOrDefault(item => item.Name == name);

        if (matchingItem is null)
            throw new ArgumentException($"No enumeration found with name {name} for type {typeof(T).Name}");

        return matchingItem;
    }

    public static bool TryFromName(string name, out Enumeration<T>? enumeration)
    {
        enumeration = GetAll().FirstOrDefault(item => item.Name == name);

        return enumeration is not null;
    }
}