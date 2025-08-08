using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.ChangeInterestRate;

[UsedImplicitly]
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
