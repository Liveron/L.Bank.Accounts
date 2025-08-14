using L.Bank.Accounts.Common;
using L.Bank.Accounts.Extensions;
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
        CancellationToken _)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<object?>(query.OwnerId);

        var account = await accountsRepository.GetAccountAsync(query.AccountId, query.OwnerId);
        if (account is null)
            return ResultFactory.FailAccountNotFound<object?>(query.AccountId);

        return !account.TryGetPropertyValue(query.PropertyName, out var propertyValue) 
            ? ResultFactory.FailPropertyNotFound<object?>(query.PropertyName) 
            : ResultFactory.Success(propertyValue);
    }
}
