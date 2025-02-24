using GameService.Domain.Dtos;
using MediatR;

namespace GameService.Domain.Queries;

public class GetRandomChoiceQuery : IRequest<Choice>
{
}