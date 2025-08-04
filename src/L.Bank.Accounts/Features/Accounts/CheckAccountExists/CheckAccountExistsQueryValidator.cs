using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.CheckAccountExists;

public class CheckAccountExistsQueryValidator : AbstractValidator<CheckAccountExistsQuery>
{
    public CheckAccountExistsQueryValidator()
    {
        RuleFor(q => q.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");
    }
}
