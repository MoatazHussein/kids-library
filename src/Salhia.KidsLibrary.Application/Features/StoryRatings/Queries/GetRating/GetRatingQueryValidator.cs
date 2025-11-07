using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;

public class GetRatingQueryValidator : AbstractValidator<GetRatingQuery>
{
    public GetRatingQueryValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty()
            .WithMessage("Master Story ID is required")
            .Length(26)
            .WithMessage("Master Story ID must be a valid ULID (26 characters)");
    }
}
