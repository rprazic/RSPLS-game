using System.Net.Http.Json;
using GameService.Domain.Responses;
using GameService.Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure.Clients;

public class RandomNumberClient(HttpClient httpClient, ILogger<RandomNumberClient> logger)
    : IRandomNumberClient
{
    public async Task<int> GetRandomNumberAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<RandomNumberResponse>(
                "random", cancellationToken);
            return response?.Random_Number ?? 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting random number from external service");
            throw;
        }
    }
}