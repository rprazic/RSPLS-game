using GameService.Infrastructure.Abstractions;
using GameService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GameService.Application.Tests.Helpers;

public class TestFixture : IAsyncLifetime
{
    public IHost? Host { get; set; }

    public async Task InitializeAsync()
    {
        var builder = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<IChoiceRepository, ChoiceRepository>();
            });
        Host = builder.Build();
        await Host.StartAsync();
    }


    public async Task DisposeAsync()
    {
        await Host.StopAsync();
        Host.Dispose();
    }
}