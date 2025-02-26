using GameService.Application.Abstractions;
using GameService.Infrastructure.Extensions;
using GameService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GameDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
        services.AddRandomNumberClient(configuration);
        services.AddSingleton<IChoiceRepository, ChoiceRepository>();

        return services;
    }
}