using GameService.Domain.Entities;
using GameService.Domain.Models;

namespace GameService.Domain.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Extends IQueryable<see cref="GameResult"/> to filter results based on a time range
    /// </summary>
    /// <param name="query">The source query to filter</param>
    /// <param name="range">Optional time range with From and To date boundaries</param>
    /// <returns>A filtered IQueryable based on the provided time range</returns>
    public static IQueryable<GameResult> ApplyFilter(this IQueryable<GameResult> query, TimeRange? range)
    {
        if (range is null)
        {
            return query;
        }

        if (range.From.HasValue)
        {
            query = query.Where(x => x.PlayedAt >= range.From.Value);
        }

        if (range.To.HasValue)
        {
            query = query.Where(x => x.PlayedAt <= range.To.Value);
        }

        return query;
    }
}