using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Behaviours;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer = new();

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;
        if (elapsedMilliseconds <= 500)
        {
            return response;
        }

        var requestName = typeof(TRequest).Name;
        logger.LogWarning(
            "Long running request: {RequestName} took {ElapsedMilliseconds} milliseconds",
            requestName,
            elapsedMilliseconds);
        return response;
    }
}