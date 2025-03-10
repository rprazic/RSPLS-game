using GameService.Application.Handlers;
using GameService.Application.Tests.Helpers;
using GameService.Domain.Entities;
using GameService.Domain.Enums;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetLatestResultsQueryHandlerTests
{
    private readonly GetLatestResultsQueryHandler _handler;

    public GetLatestResultsQueryHandlerTests()
    {
        var mockDbContext = new Mock<GameDbContext>();
        var gameResults = new List<GameResult>
        {
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = GameChoice.Rock, ComputerChoice = GameChoice.Scissors,
                Result = "win", PlayerChoiceName = "rock", ComputerChoiceName = "scissors",
                PlayedAt = DateTime.UtcNow.AddMinutes(-5)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = GameChoice.Paper, ComputerChoice = GameChoice.Rock,
                Result = "win", PlayerChoiceName = "paper", ComputerChoiceName = "rock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-4)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = GameChoice.Scissors, ComputerChoice = GameChoice.Paper,
                Result = "win", PlayerChoiceName = "scissors", ComputerChoiceName = "paper",
                PlayedAt = DateTime.UtcNow.AddMinutes(-3)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = GameChoice.Lizard, ComputerChoice = GameChoice.Spock,
                Result = "win", PlayerChoiceName = "lizard", ComputerChoiceName = "spock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-2)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = GameChoice.Spock, ComputerChoice = GameChoice.Rock,
                Result = "win", PlayerChoiceName = "spock", ComputerChoiceName = "rock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-1)
            }
        };

        var mockDbSet = MockDbSetFactory.Create(gameResults);
        mockDbContext.Setup(x => x.GameResults)
            .Returns(mockDbSet.Object);
        _handler = new GetLatestResultsQueryHandler(mockDbContext.Object);
    }

    [Fact]
    public async Task Handle_ReturnsLatestResults()
    {
        // Arrange
        var query = new GetLatestResultsQuery { Count = 3 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        // Latest results should be returned first (ordered by PlayedAt descending)
        Assert.Equal("spock", result[0].PlayerChoice);
        Assert.Equal("lizard", result[1].PlayerChoice);
        Assert.Equal("scissors", result[2].PlayerChoice);
    }

    [Fact]
    public async Task Handle_RespectsCountParameter()
    {
        // Arrange
        var query = new GetLatestResultsQuery { Count = 2 };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
    }
}