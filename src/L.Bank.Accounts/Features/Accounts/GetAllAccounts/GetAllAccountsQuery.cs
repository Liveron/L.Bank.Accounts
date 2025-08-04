using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

public sealed record GetAllAccountsQuery(Guid OwnerId) : IRequest<MbResult<List<Account>>>
{
    /// <summary>
    /// ID владельца счетов
    /// </summary>
    public Guid OwnerId { get; init; } = OwnerId;
}