using GameService.Application.Handlers;
using GameService.Application.Tests.Helpers;
using GameService.Domain.Entities;
using GameService.Domain.Enums;
using GameService.Domain.Models;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetGameStatisticsQueryHandlerTests
{
    private readonly GetGameStatisticsQueryHandler _handler;

    public GetGameStatisticsQueryHandlerTests()
    {
        var gameResults = new List<GameResult>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PlayerChoice = GameChoice.Rock,
                ComputerChoice = GameChoice.Scissors,
                Result = "win",
                PlayerChoiceName = "rock",
                ComputerChoiceName = "scissors",
                PlayedAt = new DateTime(2025, 1, 1)
            },
            new()
            {
                Id = Guid.NewGuid(),
                PlayerChoice = GameChoice.Paper,
                ComputerChoice = GameChoice.Rock,
                Result = "win",
                PlayerChoiceName = "paper",
                ComputerChoiceName = "rock",
                PlayedAt = new DateTime(2025, 2, 1)
            }
        };

        var mockDbContext = new Mock<GameDbContext>();
        var mockDbSet = MockDbSetFactory.Create(gameResults);
        mockDbContext.Setup(x => x.GameResults)
            .Returns(mockDbSet.Object);
        var mockLogger = new Mock<ILogger<GetGameStatisticsQueryHandler>>();

        _handler = new GetGameStatisticsQueryHandler(mockDbContext.Object, mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithTimeRange_FiltersResultsCorrectly()
    {
        // Arrange
        var query = new GetGameStatisticsQuery
        {
            TimeRange = new TimeRange
            {
                From = new DateTime(2025, 1, 1),
                To = new DateTime(2025, 1, 31)
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.TotalGames);
        Assert.Equal("rock", result.MostCommonPlayerChoice);
    }

    [Fact]
    public async Task Handle_WithoutTimeRange_ReturnsAllResults()
    {
        // Arrange
        var query = new GetGameStatisticsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalGames);
        Assert.Equal(100, result.WinRate); // Both games are wins
    }

    [Fact]
    public async Task Handle_WithEmptyTimeRange_ReturnsEmptyStatistics()
    {
        // Arrange
        var query = new GetGameStatisticsQuery
        {
            TimeRange = new TimeRange
            {
                From = new DateTime(2026, 1, 1), // Future date
                To = new DateTime(2026, 12, 31)
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(0, result.TotalGames);
        Assert.Equal(0, result.WinRate);
        Assert.Equal(0, result.AverageGamesPerDay);
    }
}