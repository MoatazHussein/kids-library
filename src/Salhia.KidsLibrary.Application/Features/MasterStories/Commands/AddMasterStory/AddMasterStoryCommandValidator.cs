using FluentValidation;
using Salhia.KidsLibrary.Domain.Enums;

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

        RuleFor(x => x.CoverImageUrl)
            .MaximumLength(500).WithMessage("Cover image URL must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.CoverImageUrl));

        RuleFor(x => x.MediaType)
            .NotEqual(MediaType.Unknown).WithMessage("Media type must be specified")
            .IsInEnum().WithMessage("Invalid media type");

        RuleFor(x => x.MediaUrl)
            .NotEmpty().WithMessage("Media URL is required")
            .MaximumLength(500).WithMessage("Media URL must not exceed 500 characters");

        RuleFor(x => x.PublishYear)
            .GreaterThanOrEqualTo(2000).WithMessage("Publish year must be 2000 or later")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 10).WithMessage($"Publish year cannot be more than 10 years in the future")
            .When(x => x.PublishYear.HasValue);
    }
}
