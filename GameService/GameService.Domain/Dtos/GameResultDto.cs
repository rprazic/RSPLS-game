namespace GameService.Domain.Dtos;

public class GameResultDto
{
    public DateTime PlayedAt { get; set; }
    public string PlayerChoice { get; set; } = string.Empty;
    public string ComputerChoice { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
}