using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.GetAccountProperty;

[UsedImplicitly]
public sealed class GetAccountPropertyQueryValidator : AbstractValidator<GetAccountPropertyQuery>
{
    public GetAccountPropertyQueryValidator()
    {
        RuleFor(q => q.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(q => q.OwnerId)
            .NotEmpty()
            .WithMessage("Owner ID is required.");

        RuleFor(q => q.PropertyName)
            .NotEmpty()
            .WithMessage("Property name is required");
    }
}