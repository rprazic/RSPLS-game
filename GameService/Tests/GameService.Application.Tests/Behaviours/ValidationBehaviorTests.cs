using FluentValidation;
using FluentValidation.Results;
using GameService.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class ValidationBehaviorTests
{
    private readonly Mock<IValidator<TestCommand>> _mockValidator;
    private readonly Mock<ILogger<ValidationBehavior<TestCommand, string>>> _mockLogger;
    private readonly ValidationBehavior<TestCommand, string> _behavior;

    // Test command
    public class TestCommand : IRequest<string>
    {
        public string Value { get; set; }
    }

    public ValidationBehaviorTests()
    {
        _mockValidator = new Mock<IValidator<TestCommand>>();
        _mockLogger = new Mock<ILogger<ValidationBehavior<TestCommand, string>>>();
        _behavior = new ValidationBehavior<TestCommand, string>(
            new List<IValidator<TestCommand>> { _mockValidator.Object }, 
            _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallNext()
    {
        // Arrange
        var command = new TestCommand { Value = "Valid" };
        var validationResult = new ValidationResult();
        
        _mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);
        
        bool nextCalled = false;
        Task<string> Next() 
        {
            nextCalled = true;
            return Task.FromResult("Result");
        }

        // Act
        var result = await _behavior.Handle(command, Next, CancellationToken.None);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal("Result", result);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new TestCommand { Value = "Invalid" };
        var validationFailure = new ValidationFailure("Value", "Error message");
        var validationResult = new ValidationResult(new[] { validationFailure });
        
        _mockValidator
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => 
            _behavior.Handle(command, () => Task.FromResult("Result"), CancellationToken.None));
        
        Assert.Single(exception.Errors);
        Assert.Equal("Value", exception.Errors.First().PropertyName);
        Assert.Equal("Error message", exception.Errors.First().ErrorMessage);
    }
}