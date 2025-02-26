using GameService.Domain.Dtos;
using GameService.Domain.Models;
using MediatR;

namespace GameService.Domain.Queries;

public class GetRandomChoiceQuery : IRequest<Choice>
{
}