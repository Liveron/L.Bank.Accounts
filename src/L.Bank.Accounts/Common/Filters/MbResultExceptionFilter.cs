using L.Bank.Accounts.Common.Exceptions;
using L.Bank.Accounts.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace L.Bank.Accounts.Common.Filters;

public class MbResultExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

        if (actionDescriptor is null) 
            return;

        var returnType = actionDescriptor.MethodInfo.ReturnType;

        if (!IsMbResultTask(returnType))
            return;

        var (statusCode, errorMessage) = GetErrorDetails(context.Exception);

        var errorResult = CreateErrorResult(returnType, errorMessage);

        context.Result = new ObjectResult(errorResult)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
            
        base.OnException(context);
    }

    private static bool IsMbResultTask(Type returnType)
    {
        if (!returnType.IsGenericTask())
            return false;

        var taskArgument = returnType.GetGenericArguments().First();

        return taskArgument.IsMbResult() || taskArgument.IsGenericMbResult();
    }

    private static (int statusCode, string errorMessage) GetErrorDetails(Exception exception)
    {
        return exception switch
        {
            ConcurrencyException concurrencyEx => (StatusCodes.Status409Conflict, concurrencyEx.Message),
            _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred.")
        };
    }

    private static MbResult CreateErrorResult(Type returnType, string errorMessage)
    {
        var taskArgument = returnType.GetGenericArguments().First();


        // ReSharper disable once InvertIf Предлагает менее читаемый код
        if (taskArgument.IsGenericMbResult())
        {
            var genericArgument = returnType.GetGenericArguments().First();
            return MbResult.Fail(genericArgument, errorMessage);
        }

        return MbResult.Fail(errorMessage);
    }
}