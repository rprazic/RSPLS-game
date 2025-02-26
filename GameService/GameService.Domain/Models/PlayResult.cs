namespace GameService.Domain.Models;

public class PlayResult
{
    public string Results { get; set; } = string.Empty;
    public int Player { get; set; }
    public int Computer { get; set; }
}