using GameService.Domain.Commands;
using GameService.Domain.Entities;
using GameService.Domain.Enums;
using GameService.Domain.Extensions;
using GameService.Domain.Models;
using GameService.Infrastructure;
using GameService.Infrastructure.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GameService.Application.Handlers;

public class PlayGameCommandHandler(
    IRandomNumberClient randomNumberClient,
    GameDbContext dbContext,
    ILogger<PlayGameCommandHandler> logger)
    : IRequestHandler<PlayGameCommand, PlayResult>
{
    public async Task<PlayResult> Handle(PlayGameCommand request, CancellationToken cancellationToken)
    {
        var randomNumber = await randomNumberClient.GetRandomNumberAsync(cancellationToken);
        var computerChoiceId = (randomNumber - 1) % 5 + 1;
        var computerChoice = (GameChoice)computerChoiceId;
        var playerChoice = (GameChoice)request.PlayerChoice;

        var result = DetermineWinner(playerChoice, computerChoice);

        await StoreResultAsync(playerChoice, computerChoice, result, cancellationToken);
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

    private string DetermineWinner(GameChoice player, GameChoice computer)
    {
        if (player == computer)
        {
            logger.LogInformation("Game resulted in a tie");
            return "tie";
        }

        var rules = new Dictionary<GameChoice, List<GameChoice>>
        {
            { GameChoice.Rock, [GameChoice.Scissors, GameChoice.Lizard] },
            { GameChoice.Paper, [GameChoice.Rock, GameChoice.Spock] },
            { GameChoice.Scissors, [GameChoice.Paper, GameChoice.Lizard] },
            { GameChoice.Lizard, [GameChoice.Paper, GameChoice.Spock] },
            { GameChoice.Spock, [GameChoice.Rock, GameChoice.Scissors] }
        };

        var result = rules[player].Contains(computer) ? "win" : "lose";
        logger.LogInformation(
            "Game result determined: {Result} (Player: {PlayerChoice}, Computer: {ComputerChoice})",
            result, player, computer);

        return result;
    }

    private async Task StoreResultAsync(GameChoice playerChoice, GameChoice computerChoice, string result,
        CancellationToken cancellationToken)
    {
        var gameResult = new GameResult
        {
            PlayerChoice = playerChoice,
            ComputerChoice = computerChoice,
            Result = result,
            PlayerChoiceName = playerChoice.ToDescriptionString(),
            ComputerChoiceName = computerChoice.ToDescriptionString(),
            PlayedAt = DateTime.UtcNow
        };

        dbContext.GameResults.Add(gameResult);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}