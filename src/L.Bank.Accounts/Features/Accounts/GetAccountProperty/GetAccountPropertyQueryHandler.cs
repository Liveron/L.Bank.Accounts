using System.Reflection;
using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;

namespace L.Bank.Accounts.Features.Accounts.GetAccountProperty;

public sealed class GetAccountPropertyQueryHandler(
    IAccountsRepository accountsRepository,
    IIdentityService identityService)
    : RequestHandler<GetAccountPropertyQuery, MbResult<object?>>
{
    public override async Task<MbResult<object?>> Handle(GetAccountPropertyQuery query,
        CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<object?>(query.OwnerId);

        var account = await accountsRepository.GetAccountAsync(query.AccountId, query.OwnerId);
        if (account is null)
            return ResultFactory.FailAccountNotFound<object?>(query.AccountId);

        var property = typeof(Account).GetProperty(query.PropertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        if (property is null)
            return ResultFactory.FailPropertyNotFound<object?>(query.PropertyName);

        var value = property.GetValue(account);
        return MbResult.Success(value);
    }
}
