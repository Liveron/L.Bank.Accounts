using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

public class CloseAccountCommandValidator : AbstractValidator<CloseAccountCommand>
{
    public CloseAccountCommandValidator()
    {
        RuleFor(c => c.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(c => c.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");
    }
}