namespace GameService.Infrastructure.Abstractions;

public interface IRandomNumberClient
{
    Task<int> GetRandomNumberAsync(CancellationToken cancellationToken = default);
}