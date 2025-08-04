using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

[UsedImplicitly]
public class CheckAccountExistsQueryValidator : AbstractValidator<CheckAccountExistsQuery>
{
    public CheckAccountExistsQueryValidator()
    {
        RuleFor(q => q.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");
    }
}
