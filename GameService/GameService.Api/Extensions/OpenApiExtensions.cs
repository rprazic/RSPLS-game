using Microsoft.OpenApi.Models;

namespace GameService.Api.Extensions;

public static class OpenApiExtensions
{
    /// <summary>
    /// Adds OpenAPI support to the service collection, configuring Swagger with API key authentication.
    /// </summary>
    /// <param name="services">The service collection to add OpenAPI services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("ApiKeyHeader", new OpenApiSecurityScheme()
            {
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "Authorization by x-api-key inside request's header",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKeyHeader" }
                    },
                    []
                }
            });
        });

        return services;
    }
}