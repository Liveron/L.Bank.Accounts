using System.ComponentModel.DataAnnotations;
using L.Bank.Accounts.Common;
using MediatR;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

public sealed record GetAllAccountsQuery : IRequest<MbResult<List<Account>>>
{
    /// <summary>
    /// ID владельца счетов
    /// </summary>
    public required Guid OwnerId { get; init; }
}