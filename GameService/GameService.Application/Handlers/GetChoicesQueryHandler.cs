using GameService.Domain.Models;
using GameService.Domain.Queries;
using GameService.Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class GetChoicesQueryHandler(IChoiceRepository choiceRepository, ILogger<GetChoicesQueryHandler> logger)
    : IRequestHandler<GetChoicesQuery, List<Choice>>
{
    public Task<List<Choice>> Handle(GetChoicesQuery request, CancellationToken cancellationToken)
    {
        var choices = choiceRepository.GetAllChoices();
        return Task.FromResult(choices);
    }
}