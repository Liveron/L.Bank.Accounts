using FluentValidation;

namespace L.Bank.Accounts.Features.Accounts.BlockClient;

public sealed class BlockClientCommandValidator : AbstractValidator<BlockClientCommand>
{
    public BlockClientCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .WithMessage("Id пользователя должно быть указано.");
    }
}