using GameService.Api.Middleware;

namespace GameService.Api.Extensions;

public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>
    /// Extension method that adds the custom exception handling middleware to the application's request pipeline.
    /// </summary>
    /// <param name="builder">The application builder instance.</param>
    /// <returns>The application builder instance with the exception handling middleware added.</returns>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}