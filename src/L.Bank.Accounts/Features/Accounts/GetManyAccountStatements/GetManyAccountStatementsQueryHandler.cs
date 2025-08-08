using L.Bank.Accounts.Common;
using L.Bank.Accounts.Features.Accounts.Errors;
using L.Bank.Accounts.Features.Accounts.Requests.CreateAccountStatement;
using L.Bank.Accounts.Identity;
using L.Bank.Accounts.Identity.Errors;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetManyAccountStatements;

public sealed class GetManyAccountStatementsQueryHandler(
    IMediator mediator, IIdentityService identityService, IAccountsRepository accountsRepository)
    : RequestHandler<GetManyAccountStatementsQuery, MbResult<List<AccountStatementVm>>>
{
    public override async Task<MbResult<List<AccountStatementVm>>> Handle(
        GetManyAccountStatementsQuery query, CancellationToken cancellationToken)
    {
        if (!await identityService.IdentifyUserAsync(query.OwnerId))
            return ResultFactory.FailUserNotFound<List<AccountStatementVm>>(query.OwnerId);

        if (query.AccountIds is not null && query.AccountIds.Any())
            return await GetAccountStatementsAsync(query.AccountIds, query.OwnerId, query.StartDate, query.EndDate);

        return await GetAllAccountStatementsAsync(query.OwnerId, query.StartDate, query.EndDate);
    }

    private async Task<MbResult<List<AccountStatementVm>>> GetAccountStatementsAsync(
        IEnumerable<Guid> accountIds, Guid ownerId, DateOnly startDate, DateOnly endDate)
    {
        List<AccountStatementVm> statements = [];

        foreach (var accountId in accountIds)
        {
            var account = await accountsRepository.GetAccountAsync(accountId, ownerId);
            if (account is null)
                return ResultFactory.FailAccountNotFound<List<AccountStatementVm>>(accountId);

            var request = new CreateAccountStatementRequest(account, startDate, endDate);
            var statement = await mediator.Send(request);

            statements.Add(statement);
        }

        return MbResult.Success(statements);
    }

    private async Task<MbResult<List<AccountStatementVm>>> GetAllAccountStatementsAsync(
        Guid ownerId, DateOnly startDate, DateOnly endDate)
    {
        List<AccountStatementVm> statements = [];

        var accounts = await accountsRepository.GetAllAccountsAsync(ownerId);

        foreach (var command in accounts.Select(
                     account => new CreateAccountStatementRequest(account, startDate, endDate)))
        {
            var statement = await mediator.Send(command);
            statements.Add(statement);
        }

        return MbResult.Success(statements);
    }
}