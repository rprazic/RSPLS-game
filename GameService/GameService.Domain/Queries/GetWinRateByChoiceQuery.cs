using GameService.Domain.Models;
using MediatR;

namespace GameService.Domain.Queries;

public class GetWinRateByChoiceQuery : IRequest<Dictionary<string, double>>
{
    public TimeRange? TimeRange { get; set; }
}