using L.Bank.Accounts.Common;
using L.Bank.Accounts.Database;
using L.Bank.Accounts.Features.Accounts.Errors;
using Microsoft.EntityFrameworkCore;

namespace L.Bank.Accounts.Features.Accounts.AccrueInterest;

public sealed class AccrueInterestCommandHandler(AccountsDbContext dbContext) 
    : RequestHandler<AccrueInterestCommand, MbResult>
{
    public override async Task<MbResult> Handle(AccrueInterestCommand request, CancellationToken _)
    {
        var rowsAffected = await dbContext.Database.ExecuteSqlRawAsync(
            "CALL accrue_interest({0})", request.AccountId);

        return rowsAffected > 0 ? ResultFactory.Success() : ResultFactory.FailAccountNotFound(request.AccountId);
    }
}
