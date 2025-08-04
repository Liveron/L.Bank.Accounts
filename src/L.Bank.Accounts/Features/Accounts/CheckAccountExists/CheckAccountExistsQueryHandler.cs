using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.GetAllAccounts;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

public sealed class CheckAccountExistsQueryHandler(IMediator mediator)
    : IRequestHandler<CheckAccountExistsQuery, MbResult<bool>>
{
    public async Task<MbResult<bool>> Handle(CheckAccountExistsQuery query, CancellationToken token)
    {
        var getAllAccountsQuery = new GetAllAccountsQuery(query.OwnerId);
        var result = await mediator.Send(getAllAccountsQuery, token);

        if (result.IsFailure)
            return MbResult.Fail<bool>(result.Error!);

        var accounts = result.Value!;
        if (accounts.Count == 0)
            return MbResult.Success(false);

        if (query.AccountType is null)
            return MbResult.Success(true);

        var hasAccount = accounts.Any(a => a.Type == query.AccountType);
        return MbResult.Success(hasAccount);
    }
}
