using System.Data;

namespace L.Bank.Accounts.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TransactionAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
    : Attribute
{
    public IsolationLevel IsolationLevel { get; } = isolationLevel;
}