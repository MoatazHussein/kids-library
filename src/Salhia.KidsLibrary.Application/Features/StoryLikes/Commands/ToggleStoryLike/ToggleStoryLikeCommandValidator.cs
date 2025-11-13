using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryLikes.Commands.ToggleStoryLike;

public class ToggleStoryLikeCommandValidator : AbstractValidator<ToggleStoryLikeCommand>
{
    public ToggleStoryLikeCommandValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");
    }
}
