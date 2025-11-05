using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;

public class AddStoryCommentCommandValidator : AbstractValidator<AddStoryCommentCommand>
{
    public AddStoryCommentCommandValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(2000).WithMessage("Content must not exceed 2000 characters");
    }
}
