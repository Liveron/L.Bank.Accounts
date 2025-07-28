using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.Transfer;

public sealed class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator()
    {
        RuleFor(x => x.FromAccountId)
            .NotEmpty()
            .WithMessage("Account to debit ID is required.");

        RuleFor(x => x.ToAccountId)
            .NotEmpty()
            .WithMessage("Account to credit ID is required.");

        RuleFor(x => x.Sum)
            .GreaterThan(0)
            .WithMessage("Transfer sum must be greater than 0.");
    }
}