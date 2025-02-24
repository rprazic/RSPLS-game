using GameService.Domain.Queries;
using GameService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Handlers;

public class GetWinRateByChoiceQueryHandler(GameDbContext dbContext)
    : IRequestHandler<GetWinRateByChoiceQuery, Dictionary<string, double>>
{
    public async Task<Dictionary<string, double>> Handle(GetWinRateByChoiceQuery request,
        CancellationToken cancellationToken)
    {
        var results = await dbContext.GameResults
            .GroupBy(r => r.PlayerChoiceName)
            .Select(g => new
            {
                Choice = g.Key,
                WinRate = (double)g.Count(r => r.Result == "win") / g.Count() * 100
            })
            .ToDictionaryAsync(x => x.Choice,
                x => Math.Round((double)x.WinRate, 2),
                cancellationToken);

        return results;
    }
}