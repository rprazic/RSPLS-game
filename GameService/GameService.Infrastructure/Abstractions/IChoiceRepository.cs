using GameService.Domain.Dtos;

namespace GameService.Infrastructure.Abstractions;

public interface IChoiceRepository
{
    List<Choice> GetAllChoices();
    Choice GetChoiceByIdAsync(int id);
}