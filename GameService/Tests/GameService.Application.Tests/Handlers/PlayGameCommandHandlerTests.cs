using GameService.Application.Abstractions;
using GameService.Application.Handlers;
using GameService.Domain.Commands;
using GameService.Domain.Dtos;
using GameService.Domain.Entities;
using GameService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class PlayGameCommandHandlerTests
{
    private readonly Mock<IRandomNumberClient> _mockRandomClient;
    private readonly Mock<GameDbContext> _mockDbContext;
    private readonly Mock<DbSet<GameResult>> _mockGameResults;
    private readonly PlayGameCommandHandler _handler;

    public PlayGameCommandHandlerTests()
    {
        var choices = new List<Choice>
        {
            new() { Id = 1, Name = "rock" },
            new() { Id = 2, Name = "paper" },
            new() { Id = 3, Name = "scissors" },
            new() { Id = 4, Name = "lizard" },
            new() { Id = 5, Name = "spock" }
        };

        _mockRandomClient = new Mock<IRandomNumberClient>();
        _mockDbContext = new Mock<GameDbContext>();
        _mockGameResults = new Mock<DbSet<GameResult>>();
        _mockDbContext.Setup(x => x.GameResults).Returns(_mockGameResults.Object);
        var mockLogger = new Mock<ILogger<PlayGameCommandHandler>>();
        var mockChoiceRepository = new Mock<IChoiceRepository>();
        mockChoiceRepository.Setup(x => x.GetAllChoicesAsync())
            .ReturnsAsync(choices);

        _handler = new PlayGameCommandHandler(
            _mockRandomClient.Object,
            _mockDbContext.Object,
            mockLogger.Object,
            mockChoiceRepository.Object);
    }

    [Theory]
    [InlineData(1, 3, "win")] // Rock vs Scissors = Win
    [InlineData(1, 2, "lose")] // Rock vs Paper = Lose
    [InlineData(1, 1, "tie")] // Rock vs Rock = Tie
    public async Task Handle_ReturnsCorrectResult(int playerChoice, int computerChoice, string expectedResult)
    {
        // Arrange
        _mockRandomClient.Setup(x => x.GetRandomNumberAsync(CancellationToken.None))
            .ReturnsAsync(computerChoice);
        var command = new PlayGameCommand { PlayerChoice = playerChoice };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResult, result.Results);
        Assert.Equal(playerChoice, result.Player);
        Assert.Equal(computerChoice, result.Computer);
    }

    [Fact]
    public async Task Handle_SavesGameResultToDatabase()
    {
        // Arrange
        _mockRandomClient.Setup(x => x.GetRandomNumberAsync(CancellationToken.None))
            .ReturnsAsync(2); // Computer plays paper
        var command = new PlayGameCommand { PlayerChoice = 1 }; // Player plays rock

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockGameResults.Verify(x => x.Add(
                It.Is<GameResult>(r =>
                    r.PlayerChoice == 1 &&
                    r.ComputerChoice == 2 &&
                    r.Result == "lose" &&
                    r.PlayerChoiceName == "rock" &&
                    r.ComputerChoiceName == "paper")),
            Times.Once);

        _mockDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}