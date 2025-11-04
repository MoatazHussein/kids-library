using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;

public class UpdateMediaItemCommandValidator : AbstractValidator<UpdateMediaItemCommand>
{
    public UpdateMediaItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Media Item ID is required")
            .Length(26).WithMessage("Media Item ID must be 26 characters (ULID format)");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required")
            .MaximumLength(500).WithMessage("URL must not exceed 500 characters");
    }
}
