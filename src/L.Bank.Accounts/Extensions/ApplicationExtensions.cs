using L.Bank.Accounts.Common.Middlewares;

namespace L.Bank.Accounts.Extensions;

public static class ApplicationExtensions
{
    public static void UseSwaggerOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId("swagger-ui");
        });
    }

    public static void UseMbResultAuthorization(this WebApplication app)
    {
        app.UseMiddleware<MbResultUnauthorizedMiddleware>();
    }
}
