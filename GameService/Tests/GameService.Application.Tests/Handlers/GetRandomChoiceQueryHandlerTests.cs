using GameService.Application.Handlers;
using GameService.Application.Tests.Helpers;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using GameService.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetRandomChoiceQueryHandlerTests : IClassFixture<TestFixture>
{
    private readonly Mock<IRandomNumberClient> _mockRandomClient;
    private readonly GetRandomChoiceQueryHandler _handler;

    public GetRandomChoiceQueryHandlerTests(TestFixture fixture)
    {
        var repository = fixture.Host?.Services
            .GetRequiredService<IChoiceRepository>();
        _mockRandomClient = new Mock<IRandomNumberClient>();
        var mockLogger = new Mock<ILogger<GetRandomChoiceQueryHandler>>();

        _handler = new GetRandomChoiceQueryHandler(
            _mockRandomClient.Object,
            repository!,
            mockLogger.Object);
    }

    [Theory]
    [InlineData(1, "rock")]
    [InlineData(2, "paper")]
    [InlineData(3, "scissors")]
    [InlineData(4, "lizard")]
    [InlineData(5, "spock")]
    [InlineData(6, "rock")] // Should wrap around to rock
    [InlineData(11, "rock")] // Should wrap around to rock
    public async Task Handle_ReturnsCorrectChoice(int randomNumber, string expectedChoice)
    {
        // Arrange
        _mockRandomClient.Setup(x => x.GetRandomNumberAsync(CancellationToken.None))
            .ReturnsAsync(randomNumber);

        var query = new GetRandomChoiceQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(expectedChoice, result.Name);
    }
}