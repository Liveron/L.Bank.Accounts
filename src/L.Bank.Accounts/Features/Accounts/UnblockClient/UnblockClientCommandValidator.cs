using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.UnblockClient;

[UsedImplicitly]
public sealed class UnblockClientCommandValidator : AbstractValidator<UnblockClientCommand>
{
    public UnblockClientCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .WithMessage("Id пользователя должно быть указано.");
    }
}