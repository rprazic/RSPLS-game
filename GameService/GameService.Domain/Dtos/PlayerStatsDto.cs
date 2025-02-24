namespace GameService.Domain.Dtos;

public class PlayerStatsDto
{
    public string Choice { get; set; } = string.Empty;
    public int WinCount { get; set; }
    public int LossCount { get; set; }
    public int TieCount { get; set; }
    public double WinRate { get; set; }
}