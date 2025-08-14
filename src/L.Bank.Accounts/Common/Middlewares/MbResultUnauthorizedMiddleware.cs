using JetBrains.Annotations;
using L.Bank.Accounts.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace L.Bank.Accounts.Common.Middlewares;

public sealed class MbResultUnauthorizedMiddleware(RequestDelegate next)
{
    private const string UnauthorizedErrorMessage = "Ошибка авторизации";

    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode != StatusCodes.Status401Unauthorized)
            return;

        if (context.Response.HasStarted)
            return;

        var actionDescriptor = GetActionDescriptor(context);
        if (actionDescriptor is null)
            return;

        var errorResult = CreateErrorResult(actionDescriptor.MethodInfo.ReturnType);
        if (errorResult is not null)
            await context.Response.WriteAsJsonAsync(errorResult);
    }

    private static ControllerActionDescriptor? GetActionDescriptor(HttpContext context)
    {
        return context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
    }

    private static MbResult? CreateErrorResult(Type returnType)
    {
        if (!returnType.IsGenericTask())
            return null;

        var taskArgument = returnType.GetGenericArguments().First();

        return taskArgument switch
        {
            _ when taskArgument.IsMbResult() =>
                MbResult.Fail(UnauthorizedErrorMessage),

            _ when taskArgument.IsGenericMbResult() => CreateGenericMbResult(taskArgument),

            _ => null
        };
    }

    private static MbResult CreateGenericMbResult(Type mbResultType)
    {
        var genericArgument = mbResultType.GetGenericArguments().First();
        return MbResult.Fail(genericArgument, UnauthorizedErrorMessage);
    }
}