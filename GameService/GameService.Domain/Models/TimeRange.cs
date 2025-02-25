using GameService.Domain.Entities;

namespace GameService.Domain.Models;

public class TimeRange
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public bool HasRange => From.HasValue || To.HasValue;
}