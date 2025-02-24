using GameService.Domain.Abstractions;

namespace GameService.Domain.Entities;

public class GameResult : IEntity
{
    public Guid Id { get; set; }
    public DateTime PlayedAt { get; set; }
    public int PlayerChoice { get; set; }
    public int ComputerChoice { get; set; }
    public string Result { get; set; } = string.Empty;
    public string PlayerChoiceName { get; set; } = string.Empty;
    public string ComputerChoiceName { get; set; } = string.Empty;
}