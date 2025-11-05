using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.AddFavoriteStories;

public class AddFavoriteStoriesCommandValidator : AbstractValidator<AddFavoriteStoriesCommand>
{
    public AddFavoriteStoriesCommandValidator()
    {
        RuleFor(x => x.MasterStoryIds)
            .NotEmpty().WithMessage("At least one Master Story ID is required")
            .Must(ids => ids.Count <= 50).WithMessage("Cannot add more than 50 stories at once");

        RuleForEach(x => x.MasterStoryIds)
            .NotEmpty().WithMessage("Master Story ID cannot be empty")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");
    }
}
