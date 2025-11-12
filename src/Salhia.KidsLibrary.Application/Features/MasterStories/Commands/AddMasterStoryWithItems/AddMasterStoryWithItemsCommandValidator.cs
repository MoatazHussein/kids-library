using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStoryWithItems;

public class AddMasterStoryWithItemsCommandValidator : AbstractValidator<AddMasterStoryWithItemsCommand>
{
    public AddMasterStoryWithItemsCommandValidator()
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

        // Validate MediaItems collection
        RuleFor(x => x.MediaItems)
            .NotNull().WithMessage("MediaItems list cannot be null");

        RuleForEach(x => x.MediaItems).ChildRules(item =>
        {
            item.RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Media item title is required")
                .MaximumLength(200).WithMessage("Media item title must not exceed 200 characters");

            item.RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Media item description must not exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            item.RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Media item URL is required")
                .MaximumLength(500).WithMessage("Media item URL must not exceed 500 characters");
        });
    }
}
