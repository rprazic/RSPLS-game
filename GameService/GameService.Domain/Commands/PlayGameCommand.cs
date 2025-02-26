using GameService.Domain.Dtos;
using GameService.Domain.Models;
using MediatR;

namespace GameService.Domain.Commands;

public class PlayGameCommand : IRequest<PlayResult>
{
    public int PlayerChoice { get; set; }
}