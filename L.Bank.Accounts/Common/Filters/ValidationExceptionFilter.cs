using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace L.Bank.Accounts.Common.Filters;

public sealed class ValidationExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var validationProblemDetails = new HttpValidationProblemDetails(errors);

            var result = new BadRequestObjectResult(validationProblemDetails);

            context.Result = result;
            context.ExceptionHandled = true;
        }

        base.OnException(context);
    }
}