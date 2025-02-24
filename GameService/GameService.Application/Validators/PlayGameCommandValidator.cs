using FluentValidation;
using GameService.Domain.Commands;

namespace GameService.Aplication.Validators;

public class PlayGameCommandValidator : AbstractValidator<PlayGameCommand>
{
    public PlayGameCommandValidator()
    {
        RuleFor(x => x.PlayerChoice)
            .InclusiveBetween(1, 5)
            .WithMessage("Player choice must be between 1 and 5");
    }
}