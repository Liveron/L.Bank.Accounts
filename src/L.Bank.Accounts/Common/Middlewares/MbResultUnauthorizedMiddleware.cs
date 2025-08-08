using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace L.Bank.Accounts.Common.Middlewares;

public sealed class MbResultUnauthorizedMiddleware(RequestDelegate next)
{
    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (actionDescriptor is null)
                return;

            var returnType = actionDescriptor.MethodInfo.ReturnType;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var taskArgument = returnType.GetGenericArguments().First();
                if (taskArgument == typeof(MbResult))
                {
                    var errorResult = MbResult.Fail("Ошибка авторизации");
                    await context.Response.WriteAsJsonAsync(errorResult);
                }
                else if (taskArgument.IsGenericType && taskArgument.GetGenericTypeDefinition() == typeof(MbResult<>))
                {
                    var genericArgument = taskArgument.GetGenericArguments().First();
                    var errorResult = MbResult.Fail(genericArgument, "Ошибка авторизации");
                    await context.Response.WriteAsJsonAsync(errorResult);
                }
            }
        }
    }
}