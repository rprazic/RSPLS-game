using GameService.Domain.Dtos;
using MediatR;

namespace GameService.Domain.Queries;

public class GetPlayerChoiceStatsQuery : IRequest<List<PlayerStatsDto>>
{
}