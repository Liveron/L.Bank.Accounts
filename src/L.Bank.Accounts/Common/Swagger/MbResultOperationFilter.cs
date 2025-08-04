using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace L.Bank.Accounts.Common.Swagger;

public sealed class MbResultOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;
        Type? responseType = null;

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            responseType = returnType.GetGenericArguments().First();

        if (responseType == null) 
            return;

        var schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository);

        foreach (var response in operation.Responses)
        {
            response.Value.Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = schema
                }
            };
        }
    }
}
