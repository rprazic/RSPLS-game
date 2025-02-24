using FluentValidation;
using GameService.Domain.Queries;

namespace GameService.Application.Validators;

public class GetLatestResultsQueryValidator : AbstractValidator<GetLatestResultsQuery>
{
    public GetLatestResultsQueryValidator()
    {
        RuleFor(x => x.Count)
            .InclusiveBetween(1, 100)
            .WithMessage("Count must be between 1 and 100");
    }
}