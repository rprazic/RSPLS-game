using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using GameService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameService.Application.Handlers;

public class GetLatestResultsQueryHandler(GameDbContext dbContext)
    : IRequestHandler<GetLatestResultsQuery, List<GameResultDto>>
{
    public async Task<List<GameResultDto>> Handle(GetLatestResultsQuery request,
        CancellationToken cancellationToken)
    {
        var results = await dbContext.GameResults
            .OrderByDescending(r => r.PlayedAt)
            .Take(request.Count)
            .Select(r => new GameResultDto
            {
                PlayedAt = r.PlayedAt,
                PlayerChoice = r.PlayerChoiceName,
                ComputerChoice = r.ComputerChoiceName,
                Result = r.Result
            })
            .ToListAsync(cancellationToken);
        return results;
    }
}