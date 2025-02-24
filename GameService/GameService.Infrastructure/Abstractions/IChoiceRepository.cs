using GameService.Domain.Dtos;

namespace GameService.Application.Abstractions;

public interface IChoiceRepository
{
    Task<List<Choice>> GetAllChoicesAsync();
    Task<Choice> GetChoiceByIdAsync(int id);
}