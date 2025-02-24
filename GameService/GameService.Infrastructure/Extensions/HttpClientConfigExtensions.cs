using GameService.Application.Abstractions;
using GameService.Infrastructure.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace GameService.Infrastructure.Extensions;

public static class HttpClientConfigExtensions
{
    public static IServiceCollection AddRandomNumberClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (result, timeSpan, retryCount, context) =>
                {
                    var logger = services.BuildServiceProvider()
                        .GetService<ILogger<RandomNumberClient>>();
                    logger?.LogWarning(result.Exception,
                        "Retry {RetryCount} after {TimeSpan}s delay due to {Message}",
                        retryCount,
                        timeSpan.TotalSeconds,
                        result.Exception.Message);
                });

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);
        services.AddHttpClient<IRandomNumberClient, RandomNumberClient>(client =>
            {
                client.BaseAddress = new Uri("https://codechallenge.boohma.com/");
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        return services;
    }
}