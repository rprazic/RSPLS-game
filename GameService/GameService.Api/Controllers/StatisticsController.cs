using GameService.Domain.Dtos;
using GameService.Domain.Models;
using GameService.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class StatisticsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GameStatisticsDto))]
    public async Task<ActionResult<GameStatisticsDto>> GetStatistics(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        return await mediator.Send(new GetGameStatisticsQuery
        {
            TimeRange = new TimeRange { From = from, To = to }
        });
    }

    [HttpGet("choices")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlayerStatsDto>))]
    public async Task<ActionResult<List<PlayerStatsDto>>> GetChoiceStatistics()
    {
        return await mediator.Send(new GetPlayerChoiceStatsQuery());
    }

    [HttpGet("win-rates")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, double>))]
    public async Task<ActionResult<Dictionary<string, double>>> GetWinRates()
    {
        return await mediator.Send(new GetWinRateByChoiceQuery());
    }
}