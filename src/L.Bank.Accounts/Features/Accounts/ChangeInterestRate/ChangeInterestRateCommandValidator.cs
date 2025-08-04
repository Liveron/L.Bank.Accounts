using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

public sealed class ChangeInterestRateCommandValidator : AbstractValidator<ChangeInterestRateCommand>
{
    public ChangeInterestRateCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(c => c.InterestRate)
            .NotEmpty()
            .WithMessage("Interest rate is required.");
    }
}
