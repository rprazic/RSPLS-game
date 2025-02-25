using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var requestType = typeof(TRequest).Name;
        var requestJson = JsonSerializer.Serialize(request, options);

        logger.LogInformation(
            "Handling {RequestType}\n Request: {RequestJson}",
            requestType,
            requestJson);

        try
        {
            var response = await next();
            logger.LogInformation(
                "Handled {RequestType} successfully",
                requestType);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error handling {RequestType}\n Request: {RequestJson}",
                requestType,
                requestJson);

            throw;
        }
    }
}