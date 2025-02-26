using GameService.Domain.Enums;
using GameService.Domain.Extensions;
using GameService.Domain.Models;
using GameService.Infrastructure.Abstractions;

namespace GameService.Infrastructure.Repositories;

public class ChoiceRepository : IChoiceRepository
{
    public List<Choice> GetAllChoices()
    {
        return Enum.GetValues<GameChoice>()
            .Select(choice => new Choice { Id = choice.ToInt(), Name = choice.ToDescriptionString() })
            .ToList();
    }

    public Choice GetChoiceByIdAsync(int id)
    {
        if (id is < 1 or > 5)
        {
            throw new KeyNotFoundException($"Choice with ID {id} not found");
        }

        var enumeration = (GameChoice)id;
        return new Choice { Id = id, Name = enumeration.ToDescriptionString() };
    }
}