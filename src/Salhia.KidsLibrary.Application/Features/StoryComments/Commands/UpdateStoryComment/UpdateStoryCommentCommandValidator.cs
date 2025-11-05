using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;

public class UpdateStoryCommentCommandValidator : AbstractValidator<UpdateStoryCommentCommand>
{
    public UpdateStoryCommentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Comment ID is required")
            .Length(26).WithMessage("Comment ID must be 26 characters (ULID format)");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(2000).WithMessage("Content must not exceed 2000 characters");
    }
}
