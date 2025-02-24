using GameService.Application.Abstractions;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class GetChoicesQueryHandler(IChoiceRepository choiceRepository, ILogger<GetChoicesQueryHandler> logger)
    : IRequestHandler<GetChoicesQuery, List<Choice>>
{
    public async Task<List<Choice>> Handle(GetChoicesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all game choices");
        return await choiceRepository.GetAllChoicesAsync();
    }
}