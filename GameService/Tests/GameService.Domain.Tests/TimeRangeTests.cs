using GameService.Domain.Entities;
using GameService.Domain.Models;

namespace GameService.Domain.Tests;

public class TimeRangeTests
{
    private readonly IQueryable<GameResult> _queryable;

    public TimeRangeTests()
    {
        List<GameResult> gameResults =
        [
            new() { PlayedAt = new DateTime(2025, 1, 1) },
            new() { PlayedAt = new DateTime(2025, 2, 1) },
            new() { PlayedAt = new DateTime(2025, 3, 1) }
        ];
        _queryable = gameResults.AsQueryable();
    }

    [Fact]
    public void ApplyFilter_WithFromDate_FiltersCorrectly()
    {
        // Arrange
        var timeRange = new TimeRange { From = new DateTime(2025, 2, 1) };

        // Act
        var result = timeRange.ApplyFilter(_queryable).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.True(r.PlayedAt >= timeRange.From));
    }

    [Fact]
    public void ApplyFilter_WithToDate_FiltersCorrectly()
    {
        // Arrange
        var timeRange = new TimeRange { To = new DateTime(2025, 2, 1) };

        // Act
        var result = timeRange.ApplyFilter(_queryable).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, r => Assert.True(r.PlayedAt <= timeRange.To));
    }

    [Fact]
    public void ApplyFilter_WithFromAndToDate_FiltersCorrectly()
    {
        // Arrange
        var timeRange = new TimeRange
        {
            From = new DateTime(2025, 1, 15),
            To = new DateTime(2025, 2, 15)
        };

        // Act
        var result = timeRange.ApplyFilter(_queryable).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(new DateTime(2025, 2, 1), result[0].PlayedAt);
    }

    [Fact]
    public void HasRange_ReturnsCorrectValue()
    {
        // Arrange & Act & Assert
        Assert.False(new TimeRange().HasRange);
        Assert.True(new TimeRange { From = DateTime.UtcNow }.HasRange);
        Assert.True(new TimeRange { To = DateTime.UtcNow }.HasRange);
        Assert.True(new TimeRange { From = DateTime.UtcNow, To = DateTime.UtcNow }.HasRange);
    }
}