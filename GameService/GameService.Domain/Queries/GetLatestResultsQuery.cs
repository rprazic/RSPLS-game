using GameService.Domain.Dtos;
using MediatR;

namespace GameService.Domain.Queries;

public class GetLatestResultsQuery : IRequest<List<GameResultDto>>
{
    public int Count { get; set; } = 10;
}