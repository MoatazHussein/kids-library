using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.DeleteCustomStory;

public class DeleteCustomStoryCommandValidator : AbstractValidator<DeleteCustomStoryCommand>
{
    public DeleteCustomStoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Custom Story ID is required")
            .Length(26).WithMessage("Custom Story ID must be 26 characters (ULID format)");
    }
}
