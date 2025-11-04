using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;

public class AddMasterStoryCommandValidator : AbstractValidator<AddMasterStoryCommand>
{
    public AddMasterStoryCommandValidator()
    {
        RuleFor(x => x.StoryCategoryId)
            .NotEmpty().WithMessage("Story Category ID is required")
            .Length(26).WithMessage("Story Category ID must be 26 characters (ULID format)");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Content)
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters")
            .When(x => !string.IsNullOrEmpty(x.Content));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
