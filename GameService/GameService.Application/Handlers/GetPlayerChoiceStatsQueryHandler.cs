using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class GetPlayerChoiceStatsQueryHandler(
    GameDbContext dbContext,
    ILogger<GetPlayerChoiceStatsQueryHandler> logger)
    : IRequestHandler<GetPlayerChoiceStatsQuery, List<PlayerStatsDto>>
{
    public async Task<List<PlayerStatsDto>> Handle(GetPlayerChoiceStatsQuery request,
        CancellationToken cancellationToken)
    {
        var results = await dbContext.GameResults
            .GroupBy(r => r.PlayerChoiceName)
            .Select(g => new PlayerStatsDto
            {
                Choice = g.Key,
                WinCount = g.Count(r => r.Result == "win"),
                LossCount = g.Count(r => r.Result == "lose"),
                TieCount = g.Count(r => r.Result == "tie"),
                WinRate = (double)g.Count(r => r.Result == "win") / g.Count() * 100
            })
            .ToListAsync(cancellationToken);

        logger.LogInformation("Calculated statistics for {ChoiceCount} choices", results.Count);
        return results;
    }
}