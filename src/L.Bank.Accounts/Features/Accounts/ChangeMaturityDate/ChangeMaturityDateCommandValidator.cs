using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.ChangeMaturityDate;

[UsedImplicitly]
public sealed class ChangeMaturityDateCommandValidator : AbstractValidator<ChangeMaturityDateCommand>
{
    public ChangeMaturityDateCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(c => c.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");

        RuleFor(c => c.MaturityDate)
            .NotEmpty()
            .WithMessage("Maturity date is required.")
            .GreaterThanOrEqualTo(_ => DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Maturity date must be greater or equal to current date");
    }
}
