using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Commands.AddStoryCategory;

public class AddStoryCategoryCommandValidator : AbstractValidator<AddStoryCategoryCommand>
{
    public AddStoryCategoryCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.ImageUrl));
    }
}
