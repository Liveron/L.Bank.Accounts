using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.GetAccountBalance;

[UsedImplicitly]
public sealed class GetAccountBalanceQueryValidator : AbstractValidator<GetAccountBalanceQuery>
{
    public GetAccountBalanceQueryValidator()
    {
        RuleFor(q => q.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(q => q.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");
    }
}