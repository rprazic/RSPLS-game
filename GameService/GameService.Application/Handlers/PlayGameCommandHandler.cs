using GameService.Application.Abstractions;
using GameService.Domain.Commands;
using GameService.Domain.Dtos;
using GameService.Domain.Entities;
using GameService.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class PlayGameCommandHandler(
    IRandomNumberClient randomNumberClient,
    GameDbContext dbContext,
    ILogger<PlayGameCommandHandler> logger,
    IChoiceRepository choiceRepository)
    : IRequestHandler<PlayGameCommand, PlayResult>
{
    public async Task<PlayResult> Handle(PlayGameCommand request, CancellationToken cancellationToken)
    {
        var randomNumber = await randomNumberClient.GetRandomNumberAsync(cancellationToken);
        var computerChoiceId = (randomNumber - 1) % 5 + 1;

        var choices = await choiceRepository.GetAllChoicesAsync();
        var playerChoice = choices.First(c => c.Id == request.PlayerChoice);
        var computerChoice = choices.First(c => c.Id == computerChoiceId);

        var result = DetermineWinner(request.PlayerChoice, computerChoiceId);

        // Store the result in database
        var gameResult = new GameResult
        {
            PlayerChoice = request.PlayerChoice,
            ComputerChoice = computerChoiceId,
            Result = result,
            PlayerChoiceName = playerChoice.Name,
            ComputerChoiceName = computerChoice.Name,
            PlayedAt = DateTime.UtcNow
        };

        dbContext.GameResults.Add(gameResult);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Game completed and saved. Result: {Result}, Player: {PlayerChoice}, Computer: {ComputerChoice}",
            result, request.PlayerChoice, computerChoiceId);

        return new PlayResult
        {
            Results = result,
            Player = request.PlayerChoice,
            Computer = computerChoiceId
        };
    }

    private string DetermineWinner(int player, int computer)
    {
        if (player == computer)
        {
            logger.LogInformation("Game resulted in a tie");
            return "tie";
        }

        var rules = new Dictionary<int, List<int>>
        {
            { 1, new List<int> { 3, 4 } },
            { 2, new List<int> { 1, 5 } },
            { 3, new List<int> { 2, 4 } },
            { 4, new List<int> { 2, 5 } },
            { 5, new List<int> { 1, 3 } }
        };

        var result = rules[player].Contains(computer) ? "win" : "lose";
        logger.LogInformation(
            "Game result determined: {Result} (Player: {PlayerChoice}, Computer: {ComputerChoice})",
            result, player, computer);

        return result;
    }
}