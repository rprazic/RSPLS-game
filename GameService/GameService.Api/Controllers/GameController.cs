using GameService.Domain.Commands;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameService.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class GameController(IMediator mediator) : ControllerBase
{
    [HttpGet("choices")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Choice>))]
    public async Task<ActionResult<List<Choice>>> GetChoices()
    {
        return await mediator.Send(new GetChoicesQuery());
    }

    [HttpGet("choice")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Choice))]
    public async Task<ActionResult<Choice>> GetChoice()
    {
        return await mediator.Send(new GetRandomChoiceQuery());
    }

    [HttpPost("play")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayResult))]
    public async Task<ActionResult<PlayResult>> Play([FromBody] PlayRequest request)
    {
        return await mediator.Send(new PlayGameCommand { PlayerChoice = request.Player });
    }

    [HttpGet("results/latest")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GameResultDto>))]
    public async Task<ActionResult<List<GameResultDto>>> GetLatestResults([FromQuery] int count = 10)
    {
        return await mediator.Send(new GetLatestResultsQuery { Count = count });
    }
}