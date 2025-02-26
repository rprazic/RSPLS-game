using GameService.Domain.Dtos;
using GameService.Domain.Models;

namespace GameService.Infrastructure.Abstractions;

public interface IChoiceRepository
{
    List<Choice> GetAllChoices();
    Choice GetChoiceByIdAsync(int id);
}