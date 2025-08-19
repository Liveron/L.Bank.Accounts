namespace L.Bank.Accounts.Extensions;

public static class EnvironmentExtensions
{
    public static bool IsTesting(this IWebHostEnvironment env)
    {
        return env.IsEnvironment("Testing");
    }
}