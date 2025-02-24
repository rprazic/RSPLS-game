using GameService.Application.Abstractions;
using GameService.Domain.Dtos;
using GameService.Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class GetRandomChoiceQueryHandler : IRequestHandler<GetRandomChoiceQuery, Choice>
{
    private readonly IRandomNumberClient _randomNumberClient;
    private readonly IChoiceRepository _choiceRepository;
    private readonly ILogger<GetRandomChoiceQueryHandler> _logger;

    public GetRandomChoiceQueryHandler(
        IRandomNumberClient randomNumberClient,
        IChoiceRepository choiceRepository,
        ILogger<GetRandomChoiceQueryHandler> logger)
    {
        _randomNumberClient = randomNumberClient;
        _choiceRepository = choiceRepository;
        _logger = logger;
    }

    public async Task<Choice> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
    {
        var randomNumber = await _randomNumberClient.GetRandomNumberAsync(cancellationToken);
        var choiceIndex = (randomNumber - 1) % 5 + 1;
        var choices = await _choiceRepository.GetAllChoicesAsync();
        var choice = choices.First(c => c.Id == choiceIndex);

        _logger.LogInformation(
            "Generated random choice: {ChoiceName} (ID: {ChoiceId}) from random number {RandomNumber}",
            choice.Name, choice.Id, randomNumber);
        return choice;
    }
}