using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using GameService.Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class GetRandomChoiceQueryHandler(
    IRandomNumberClient randomNumberClient,
    IChoiceRepository choiceRepository,
    ILogger<GetRandomChoiceQueryHandler> logger)
    : IRequestHandler<GetRandomChoiceQuery, Choice>
{
    public async Task<Choice> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        var randomNumber = await randomNumberClient.GetRandomNumberAsync(cancellationToken);
        var choiceIndex = (randomNumber - 1) % 5 + 1;
        var choice = choiceRepository.GetChoiceByIdAsync(choiceIndex);

        logger.LogInformation(
            "Generated random choice: {ChoiceName} (ID: {ChoiceId}) from random number {RandomNumber}",
            choice.Name, choice.Id, randomNumber);
        return choice;
    }
}