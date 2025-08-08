namespace L.Bank.Accounts.Identity;

public interface IIdentityService
{
    public Task<bool> IdentifyUserAsync(Guid userId);
}

public class IdentityService : IIdentityService
{
    public Task<bool> IdentifyUserAsync(Guid userId)
    {
        return Task.FromResult(true);
    }
}