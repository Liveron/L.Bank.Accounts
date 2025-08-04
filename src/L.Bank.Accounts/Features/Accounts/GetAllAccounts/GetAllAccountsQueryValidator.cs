using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.GetAllAccounts;

[UsedImplicitly]
public class GetAllAccountsQueryValidator : AbstractValidator<GetAllAccountsQuery>
{
    public GetAllAccountsQueryValidator()
    {
        RuleFor(c => c.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");
    }
}