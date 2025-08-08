using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

public sealed record GetAllAccountsQuery : IRequest<MbResult<List<AccountVm>>>
{
    /// <summary>
    /// ID владельца счетов
    /// </summary>
    public required Guid OwnerId { get; init; }
}