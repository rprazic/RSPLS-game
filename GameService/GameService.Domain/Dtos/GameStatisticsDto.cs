namespace GameService.Domain.Dtos;

public class GameStatisticsDto
{
    public int TotalGames { get; set; }
    public Dictionary<string, int> ResultDistribution { get; set; } = new();
    public Dictionary<string, int> PlayerChoiceDistribution { get; set; } = new();
    public Dictionary<string, int> ComputerChoiceDistribution { get; set; } = new();
    public double WinRate { get; set; }
    public string MostCommonPlayerChoice { get; set; } = string.Empty;
    public string MostCommonComputerChoice { get; set; } = string.Empty;
    public double AverageGamesPerDay { get; set; }
}