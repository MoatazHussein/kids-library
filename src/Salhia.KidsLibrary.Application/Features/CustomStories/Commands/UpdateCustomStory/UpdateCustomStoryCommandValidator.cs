using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.UpdateCustomStory;

public class UpdateCustomStoryCommandValidator : AbstractValidator<UpdateCustomStoryCommand>
{
    public UpdateCustomStoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Custom Story ID is required")
            .Length(26).WithMessage("Custom Story ID must be 26 characters (ULID format)");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(200).WithMessage("Author name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
