using GameService.Domain.Dtos;
using MediatR;

namespace GameService.Domain.Queries;

public class GetChoicesQuery : IRequest<List<Choice>>
{
}