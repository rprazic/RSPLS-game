using GameService.Domain.Abstractions;
using GameService.Domain.Enums;

namespace GameService.Domain.Entities;

public class GameResult : IEntity
{
    public Guid Id { get; set; }
    public DateTime PlayedAt { get; set; }
    public GameChoice PlayerChoice { get; set; }
    public GameChoice ComputerChoice { get; set; }
    public string Result { get; set; } = string.Empty;
    public string PlayerChoiceName { get; set; } = string.Empty;
    public string ComputerChoiceName { get; set; } = string.Empty;
}