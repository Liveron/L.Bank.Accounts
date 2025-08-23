using System.Reflection;
using L.Bank.Accounts.Infrastructure.Integration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace L.Bank.Accounts.Common.Swagger;

public sealed class IntegrationEventDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var integrationEventTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IntegrationEvent).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList();


        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator Предлагает не читаемый код
        foreach (var eventType in integrationEventTypes)
        {
            var schemaId = eventType.Name;
            if (!swaggerDoc.Components.Schemas.ContainsKey(schemaId))
            {
                context.SchemaGenerator.GenerateSchema(eventType, context.SchemaRepository);
            }
        }
    }
}