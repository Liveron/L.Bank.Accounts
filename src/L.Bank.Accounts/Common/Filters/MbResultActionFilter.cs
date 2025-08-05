using L.Bank.Accounts.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace L.Bank.Accounts.Common.Filters;

public class MbResultActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint is null) 
            return;

        var successStatusCode = endpoint.Metadata
            .GetOrderedMetadata<ProducesResponseTypeAttribute>()
            .Where(x => x.StatusCode is >= 200 and < 300)
            .Select(x => x.StatusCode)
            .FirstOrDefault();

        if (successStatusCode == 0)
            throw new InvalidOperationException();

        if (context.Result is ObjectResult objectResult)
        {
            context.Result = objectResult.Value switch
            {
                MbResult { IsSuccess: true } mbResult => new ObjectResult(mbResult) { StatusCode = successStatusCode },
                MbResult { IsFailure: true } mbResultError => mbResultError.Error switch
                {
                    NotFoundError => new NotFoundObjectResult(mbResultError),
                    _ => new BadRequestObjectResult(mbResultError)
                },
                _ => context.Result
            };
        }

        base.OnActionExecuted(context);
    }
}