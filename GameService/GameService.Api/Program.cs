using GameService.Api.Extensions;
using GameService.Application;
using GameService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
        new JsonFormatter(),
        Path.Combine("logs", "game-service-.log"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        fileSizeLimitBytes: 10 * 1024 * 1024)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services
    .AddAuthentication(builder.Configuration)
    .AddOpenApiConfig()
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Disable automatic model state validation to use our custom validation behavior
        options.SuppressModelStateInvalidFilter = true;
    });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GameDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();
app.UseCors(policyBuilder =>
{
    policyBuilder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});
app.MapHealthChecks("/health");

app.Run();