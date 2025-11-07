using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.DeleteRating;

public class DeleteRatingCommandValidator : AbstractValidator<DeleteRatingCommand>
{
    public DeleteRatingCommandValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty()
            .WithMessage("Master Story ID is required")
            .Length(26)
            .WithMessage("Master Story ID must be a valid ULID (26 characters)");
    }
}
