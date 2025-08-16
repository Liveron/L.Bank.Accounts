namespace L.Bank.Accounts.Infrastructure.Identity;

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