using GameService.Application.Validators;
using GameService.Domain.Queries;

namespace GameService.Application.Tests.Validators;

public class GetLatestResultsQueryValidatorTests
{
    private readonly GetLatestResultsQueryValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task Validate_WithValidCount_ShouldNotHaveValidationError(int count)
    {
        // Arrange
        var query = new GetLatestResultsQuery { Count = count };

        // Act
        var result = await _validator.ValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(-1)]
    [InlineData(1000)]
    public async Task Validate_WithInvalidCount_ShouldHaveValidationError(int count)
    {
        // Arrange
        var query = new GetLatestResultsQuery { Count = count };

        // Act
        var result = await _validator.ValidateAsync(query);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("Count", result.Errors.First().PropertyName);
        Assert.Contains("must be between 1 and 100", result.Errors.First().ErrorMessage);
    }
}