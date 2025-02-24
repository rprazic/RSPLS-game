using System.Net;
using System.Text.Json;
using GameService.Domain.Responses;
using GameService.Infrastructure.Clients;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace GameService.Infrastructure.Test;

public class RandomNumberClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<ILogger<RandomNumberClient>> _mockLogger;
    private readonly RandomNumberClient _client;

    public RandomNumberClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockLogger = new Mock<ILogger<RandomNumberClient>>();

        var client = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://codechallenge.boohma.com/")
        };

        _client = new RandomNumberClient(client, _mockLogger.Object);
    }

    [Fact]
    public async Task GetRandomNumberAsync_ReturnsValidNumber()
    {
        // Arrange
        var response = new RandomNumberResponse { Random_Number = 42 };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(response))
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _client.GetRandomNumberAsync();

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task GetRandomNumberAsync_WhenServiceFails_ThrowsException()
    {
        // Arrange
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Service unavailable"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetRandomNumberAsync());

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error getting random number")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}