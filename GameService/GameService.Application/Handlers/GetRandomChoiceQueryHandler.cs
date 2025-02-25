using GameService.Application.Abstractions;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
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
        var choices = await choiceRepository.GetAllChoicesAsync();
        var choice = choices.First(c => c.Id == choiceIndex);

        logger.LogInformation(
            "Generated random choice: {ChoiceName} (ID: {ChoiceId}) from random number {RandomNumber}",
            choice.Name, choice.Id, randomNumber);
        return choice;
    }
}