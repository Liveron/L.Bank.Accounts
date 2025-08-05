using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace L.Bank.Accounts.Extensions;

public static class SwaggerExtensions
{
    public static void AddKeycloakAuth(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "client_id = \"swagger-ui\", username = \"dev\", password = \"dev\"",
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                Implicit = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("http://localhost:8080/realms/dev-realm/protocol/openid-connect/auth"),
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    },

                },
                new List<string>()
            }
        });
    }
}