using GameService.Domain.Entities;

namespace GameService.Domain.Models;

public class TimeRange
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public bool HasRange => From.HasValue || To.HasValue;

    public IQueryable<GameResult> ApplyFilter(IQueryable<GameResult> query)
    {
        if (From.HasValue)
        {
            query = query.Where(x => x.PlayedAt >= From.Value);
        }

        if (To.HasValue)
        {
            query = query.Where(x => x.PlayedAt <= To.Value);
        }

        return query;
    }
}