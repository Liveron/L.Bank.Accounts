using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

public sealed class UnblockClientCommandValidator : AbstractValidator<UnblockClientCommand>
{
    public UnblockClientCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .WithMessage("Id пользователя должно быть указано.");
    }
}