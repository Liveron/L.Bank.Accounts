using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.CreateTransaction;

[UsedImplicitly]
public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(a => a.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(c => c.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");

        RuleFor(x => x.TransactionType)
            .IsInEnum()
            .WithMessage("Unknown transaction type");

        RuleFor(a => a.Sum)
            .GreaterThan(0)
            .WithMessage("Transaction sum must be greater than 0.");
    }
}