using FluentValidation;
using JetBrains.Annotations;

namespace L.Bank.Accounts.Features.Accounts.GetAccountStatement;

[UsedImplicitly]
public class GetAccountStatementQueryValidator : AbstractValidator<GetAccountStatementQuery>
{
    public GetAccountStatementQueryValidator()
    {
        RuleFor(q => q.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(q => q.OwnerId)
            .NotEmpty()
            .WithMessage("Account owner ID is required.");

        RuleFor(q => q.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .LessThanOrEqualTo(q => q.EndDate)
            .WithMessage("Start date must be less than or equal to end date.");

        RuleFor(q => q.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThanOrEqualTo(q => q.StartDate)
            .WithMessage("End date must be greater than or equal to start date.");
    }
}