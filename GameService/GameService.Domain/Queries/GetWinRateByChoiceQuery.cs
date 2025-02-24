using MediatR;

namespace GameService.Domain.Queries;

public class GetWinRateByChoiceQuery : IRequest<Dictionary<string, double>>
{
}