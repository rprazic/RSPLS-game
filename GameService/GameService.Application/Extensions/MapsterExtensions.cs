using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Application.Extensions;

public static class MapsterExtensions
{
    /// <summary>
    /// Registers Mapster configuration with the dependency injection container.
    /// Scans the executing assembly for mapping configurations and registers the global settings as a singleton.
    /// </summary>
    /// <param name="services">The service collection to add Mapster to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        return services;
    }
}