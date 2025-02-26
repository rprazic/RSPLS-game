using FluentAssertions;
using GameService.Application.Handlers;
using GameService.Application.Tests.Helpers;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using GameService.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace GameService.Application.Tests.Handlers;

public class GetChoicesQueryHandlerTests : IClassFixture<TestFixture>
{
    private readonly GetChoicesQueryHandler _handler;
    private readonly List<Choice> _choices;

    public GetChoicesQueryHandlerTests(TestFixture fixture)
    {
        _choices =
        [
            new Choice { Id = 1, Name = "rock" },
            new Choice { Id = 2, Name = "paper" },
            new Choice { Id = 3, Name = "scissors" },
            new Choice { Id = 4, Name = "lizard" },
            new Choice { Id = 5, Name = "spock" }
        ];

        var repository = fixture.Host?.Services
            .GetRequiredService<IChoiceRepository>();
        var mockLogger = new Mock<ILogger<GetChoicesQueryHandler>>();

        _handler = new GetChoicesQueryHandler(repository!, mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllChoices()
    {
        // Arrange
        var query = new GetChoicesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(_choices);
    }
}