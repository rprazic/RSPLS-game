using GameService.Application.Abstractions;
using GameService.Domain.Dtos;
using GameService.Domain.Enums;
using GameService.Domain.Extensions;

namespace GameService.Infrastructure.Repositories;

public class ChoiceRepository : IChoiceRepository
{
    private readonly List<Choice> _choices =
    [
        new() { Id = 1, Name = "rock" },
        new() { Id = 2, Name = "paper" },
        new() { Id = 3, Name = "scissors" },
        new() { Id = 4, Name = "lizard" },
        new() { Id = 5, Name = "spock" }
    ];

    public Task<List<Choice>> GetAllChoicesAsync()
    {
        var choices = Enum.GetValues<GameChoice>()
            .Select(choice => new Choice { Id = choice.ToInt(), Name = choice.ToDescriptionString() })
            .ToList();
        return Task.FromResult(choices);
    }

    public Task<Choice> GetChoiceByIdAsync(int id)
    {
        var choice = _choices.FirstOrDefault(c => c.Id == id);
        if (choice is null)
        {
            throw new KeyNotFoundException($"Choice with ID {id} not found");
        }

        return Task.FromResult(choice);
    }
}