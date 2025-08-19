using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

[UsedImplicitly]
public sealed class BlockClientCommandValidator : AbstractValidator<BlockClientCommand>
{
    public BlockClientCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .WithMessage("Id пользователя должно быть указано.");
    }
}