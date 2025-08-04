using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.OpenAccount;

public class OpenAccountCommandValidator : AbstractValidator<OpenAccountCommand>
{
    public OpenAccountCommandValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .WithMessage("Owner ID is required.");

        RuleFor(x => x.AccountTerms)
            .Must(BeAccountTerms)
            .WithMessage("Unknown account terms.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.");
        
        RuleFor(x => x.Currency)
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be a valid 3-letter ISO currency code.");

        RuleFor(x => x.MaturityDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Дата погашения счета не может быть раньше текущей.");
    }

    private static bool BeAccountTerms(string name) => AccountTerms.TryFromName(name, out _);
}