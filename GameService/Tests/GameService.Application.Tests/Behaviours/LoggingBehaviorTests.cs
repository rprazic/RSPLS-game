using GameService.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class LoggingBehaviorTests
{
    private readonly Mock<ILogger<LoggingBehavior<TestCommand, string>>> _mockLogger;
    private readonly LoggingBehavior<TestCommand, string> _behavior;

    // Test command
    public class TestCommand : IRequest<string>
    {
        public string Value { get; set; }
    }

    public LoggingBehaviorTests()
    {
        _mockLogger = new Mock<ILogger<LoggingBehavior<TestCommand, string>>>();
        _behavior = new LoggingBehavior<TestCommand, string>(_mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldLogRequestAndResponse()
    {
        // Arrange
        var command = new TestCommand { Value = "Test" };

        var nextCalled = false;

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

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Handling TestCommand")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Handled TestCommand successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldLogError()
    {
        // Arrange
        var command = new TestCommand { Value = "Test" };
        var expectedException = new InvalidOperationException("Test exception");

        Task<string> Next()
        {
            throw expectedException;
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _behavior.Handle(command, Next, CancellationToken.None));

        Assert.Same(expectedException, exception);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error handling TestCommand")),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}