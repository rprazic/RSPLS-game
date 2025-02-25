using GameService.Domain.Dtos;
using GameService.Domain.Extensions;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameService.Aplication.Handlers;

public class GetGameStatisticsQueryHandler(
    GameDbContext dbContext,
    ILogger<GetGameStatisticsQueryHandler> logger)
    : IRequestHandler<GetGameStatisticsQuery, GameStatisticsDto>
{
    public async Task<GameStatisticsDto> Handle(
        GetGameStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.GameResults
            .ApplyFilter(request.TimeRange);
        var results = await query.ToListAsync(cancellationToken);

        if (!results.Any())
        {
            return new GameStatisticsDto
            {
                TotalGames = 0,
                WinRate = 0,
                AverageGamesPerDay = 0
            };
        }

        var statistics = new GameStatisticsDto
        {
            TotalGames = results.Count,
            ResultDistribution = results
                .GroupBy(r => r.Result)
                .ToDictionary(g => g.Key, g => g.Count()),
            PlayerChoiceDistribution = results
                .GroupBy(r => r.PlayerChoiceName)
                .ToDictionary(g => g.Key, g => g.Count()),
            ComputerChoiceDistribution = results
                .GroupBy(r => r.ComputerChoiceName)
                .ToDictionary(g => g.Key, g => g.Count()),
            WinRate = (double)results.Count(r => r.Result == "win") / results.Count * 100,
            MostCommonPlayerChoice = results
                .GroupBy(r => r.PlayerChoiceName)
                .OrderByDescending(g => g.Count())
                .First().Key,
            MostCommonComputerChoice = results
                .GroupBy(r => r.ComputerChoiceName)
                .OrderByDescending(g => g.Count())
                .First().Key,
            AverageGamesPerDay = results
                .GroupBy(r => r.PlayedAt.Date)
                .Average(g => g.Count())
        };

        logger.LogInformation(
            "Statistics calculated: {TotalGames} games, {WinRate}% win rate in TimeRange: {TimeRange}",
            statistics.TotalGames,
            statistics.WinRate,
            request.TimeRange?.HasRange);

        return statistics;
    }
}