using GameService.Aplication.Validators;
using GameService.Domain.Commands;

namespace GameService.Tests.Validators;

public class PlayGameCommandValidatorTests
{
    private readonly PlayGameCommandValidator _validator = new();

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validate_WithValidChoices_ShouldNotHaveValidationError(int choice)
    {
        // Arrange
        var command = new PlayGameCommand { PlayerChoice = choice };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    [InlineData(10)]
    public async Task Validate_WithInvalidChoices_ShouldHaveValidationError(int choice)
    {
        // Arrange
        var command = new PlayGameCommand { PlayerChoice = choice };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal("PlayerChoice", result.Errors.First().PropertyName);
        Assert.Contains("must be between 1 and 5", result.Errors.First().ErrorMessage);
    }
}