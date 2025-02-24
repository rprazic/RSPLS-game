namespace GameService.Application.Abstractions;

public interface IRandomNumberClient
{
    Task<int> GetRandomNumberAsync(CancellationToken cancellationToken = default);
}