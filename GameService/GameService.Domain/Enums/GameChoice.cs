using System.ComponentModel;

namespace GameService.Domain.Enums;

public enum GameChoice
{
    [Description("rock")] Rock = 1,

    [Description("paper")] Paper = 2,

    [Description("scissors")] Scissors = 3,

    [Description("lizard")] Lizard = 4,

    [Description("spock")] Spock = 5,
}