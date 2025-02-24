namespace GameService.Domain.Dtos;

public class PlayResult
{
    public string Results { get; set; } = string.Empty;
    public int Player { get; set; }
    public int Computer { get; set; }
}