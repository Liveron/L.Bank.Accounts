using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.GetAllAccounts;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

public sealed class CheckAccountExistsQueryHandler(IMediator mediator)
    : RequestHandler<CheckAccountExistsQuery, MbResult<bool>>
{
    public override async Task<MbResult<bool>> Handle(CheckAccountExistsQuery query, CancellationToken token)
    {
        var getAllAccountsQuery = new GetAllAccountsQuery { OwnerId = query.OwnerId };
        var result = await mediator.Send(getAllAccountsQuery, token);

        if (result.IsFailure)
            return ResultFactory.Fail<bool>(result.Error!);

        var accounts = result.Value!;
        if (accounts.Count == 0)
            return ResultFactory.Success(value: false);

        if (query.AccountType is null)
            return ResultFactory.Success(value: true);

        var hasAccount = accounts.Any(a => a.AccountType == query.AccountType);
        return ResultFactory.Success(value: hasAccount);
    }
}
