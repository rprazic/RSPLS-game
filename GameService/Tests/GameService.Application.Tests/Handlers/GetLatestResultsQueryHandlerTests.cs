using GameService.Application.Handlers;
using GameService.Application.Tests.Handlers;
using GameService.Domain.Entities;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
                Id = Guid.NewGuid(), PlayerChoice = 1, ComputerChoice = 3, Result = "win",
                PlayerChoiceName = "rock", ComputerChoiceName = "scissors",
                PlayedAt = DateTime.UtcNow.AddMinutes(-5)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = 2, ComputerChoice = 1, Result = "win",
                PlayerChoiceName = "paper", ComputerChoiceName = "rock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-4)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = 3, ComputerChoice = 2, Result = "win",
                PlayerChoiceName = "scissors", ComputerChoiceName = "paper",
                PlayedAt = DateTime.UtcNow.AddMinutes(-3)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = 4, ComputerChoice = 5, Result = "win",
                PlayerChoiceName = "lizard", ComputerChoiceName = "spock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-2)
            },
            new()
            {
                Id = Guid.NewGuid(), PlayerChoice = 5, ComputerChoice = 1, Result = "win",
                PlayerChoiceName = "spock", ComputerChoiceName = "rock",
                PlayedAt = DateTime.UtcNow.AddMinutes(-1)
            }
        };

        // Set up mock DbSet with LINQ support
        var mockDbSet = MockDbSet(gameResults);
        mockDbContext.Setup(x => x.GameResults).Returns(mockDbSet.Object);

        _handler = new GetLatestResultsQueryHandler(mockDbContext.Object);
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