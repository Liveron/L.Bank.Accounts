using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.GetAccountBalance;

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