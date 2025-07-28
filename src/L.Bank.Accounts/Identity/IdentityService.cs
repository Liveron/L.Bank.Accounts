namespace L.Bank.Accounts.Identity;

public interface IIdentityService
{
    public bool IdentifyUser(Guid userId);
}

public class IdentityService : IIdentityService
{
    public bool IdentifyUser(Guid userId)
    {
        return true;
    }
}