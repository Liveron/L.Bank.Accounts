using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.CloseAccount;

[UsedImplicitly]
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