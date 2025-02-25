using GameService.Application.Abstractions;
using GameService.Application.Handlers;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetRandomChoiceQueryHandlerTests
{
    private readonly Mock<IRandomNumberClient> _mockRandomClient;
    private readonly GetRandomChoiceQueryHandler _handler;

    public GetRandomChoiceQueryHandlerTests()
    {
        _mockRandomClient = new Mock<IRandomNumberClient>();
        var mockRepository = new Mock<IChoiceRepository>();
        var mockLogger = new Mock<ILogger<GetRandomChoiceQueryHandler>>();

        var choices = new List<Choice>
        {
            new() { Id = 1, Name = "rock" },
            new() { Id = 2, Name = "paper" },
            new() { Id = 3, Name = "scissors" },
            new() { Id = 4, Name = "lizard" },
            new() { Id = 5, Name = "spock" }
        };

        mockRepository.Setup(x => x.GetAllChoicesAsync())
            .ReturnsAsync(choices);
        _handler = new GetRandomChoiceQueryHandler(
            _mockRandomClient.Object,
            mockRepository.Object,
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