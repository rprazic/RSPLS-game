using GameService.Application.Abstractions;
using GameService.Application.Handlers;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetChoicesQueryHandlerTests
{
    private readonly GetChoicesQueryHandler _handler;
    private readonly List<Choice> _choices;

    public GetChoicesQueryHandlerTests()
    {
        var mockRepository = new Mock<IChoiceRepository>();
        var mockLogger = new Mock<ILogger<GetChoicesQueryHandler>>();

        _choices = new List<Choice>
        {
            new() { Id = 1, Name = "rock" },
            new() { Id = 2, Name = "paper" },
            new() { Id = 3, Name = "scissors" },
            new() { Id = 4, Name = "lizard" },
            new() { Id = 5, Name = "spock" }
        };

        mockRepository.Setup(x => x.GetAllChoicesAsync())
            .ReturnsAsync(_choices);

        _handler = new GetChoicesQueryHandler(mockRepository.Object, mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllChoices()
    {
        // Arrange
        var query = new GetChoicesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(5, result.Count);
        Assert.Equal(_choices, result);
    }
}