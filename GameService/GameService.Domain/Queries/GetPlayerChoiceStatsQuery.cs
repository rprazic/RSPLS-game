using GameService.Domain.Dtos;
using GameService.Domain.Models;
using MediatR;

namespace GameService.Domain.Queries;

public class GetPlayerChoiceStatsQuery : IRequest<List<PlayerStatsDto>>
{
    public TimeRange? TimeRange { get; set; }
}