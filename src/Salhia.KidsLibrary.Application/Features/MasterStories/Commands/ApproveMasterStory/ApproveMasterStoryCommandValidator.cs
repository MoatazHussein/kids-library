using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.ApproveMasterStory;

public class ApproveMasterStoryCommandValidator : AbstractValidator<ApproveMasterStoryCommand>
{
    public ApproveMasterStoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");
    }
}
