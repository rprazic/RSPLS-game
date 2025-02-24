using GameService.Domain.Dtos;
using GameService.Domain.Models;
using MediatR;

namespace GameService.Domain.Queries;

public class GetGameStatisticsQuery : IRequest<GameStatisticsDto>
{
    public TimeRange? TimeRange { get; set; }
}