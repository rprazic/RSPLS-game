using GameService.Aplication.Handlers;
using GameService.Domain.Entities;
using GameService.Domain.Models;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetGameStatisticsQueryHandlerTests
{
    private readonly GetGameStatisticsQueryHandler _handler;

    public GetGameStatisticsQueryHandlerTests()
    {
        var mockDbContext = new Mock<GameDbContext>();
        var mockLogger = new Mock<ILogger<GetGameStatisticsQueryHandler>>();
        var gameResults = new List<GameResult>
        {
            new()
            {
                Id = Guid.NewGuid(),
                PlayerChoice = 1,
                ComputerChoice = 3,
                Result = "win",
                PlayerChoiceName = "rock",
                ComputerChoiceName = "scissors",
                PlayedAt = new DateTime(2025, 1, 1)
            },
            new()
            {
                Id = Guid.NewGuid(),
                PlayerChoice = 2,
                ComputerChoice = 1,
                Result = "win",
                PlayerChoiceName = "paper",
                ComputerChoiceName = "rock",
                PlayedAt = new DateTime(2025, 2, 1)
            }
        };

        var mockDbSet = MockDbSet(gameResults);
        mockDbContext.Setup(x => x.GameResults).Returns(mockDbSet.Object);

        _handler = new GetGameStatisticsQueryHandler(mockDbContext.Object, mockLogger.Object);
    }

    private static Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
    {
        var queryable = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        return mockSet;
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