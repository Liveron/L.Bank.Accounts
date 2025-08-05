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
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            var taskArgument = returnType.GetGenericArguments().First();
            if (taskArgument == typeof(MbResult))
            {
                var errorResult = MbResult.Fail(context.Exception.Message);
                context.Result = new ObjectResult(errorResult)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
            else if (taskArgument.IsGenericType && taskArgument.GetGenericTypeDefinition() == typeof(MbResult<>))
            {
                var genericArgument = taskArgument.GetGenericArguments().First();
                var errorResult = MbResult.Fail(genericArgument, context.Exception.Message);
                context.Result = new ObjectResult(errorResult)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        context.ExceptionHandled = true;
            
        base.OnException(context);
    }
}