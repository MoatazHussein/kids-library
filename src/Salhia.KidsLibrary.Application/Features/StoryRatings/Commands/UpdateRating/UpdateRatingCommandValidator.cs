using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.UpdateRating;

public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty()
            .WithMessage("Master Story ID is required")
            .Length(26)
            .WithMessage("Master Story ID must be a valid ULID (26 characters)");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5 stars");
    }
}
