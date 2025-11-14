using FluentValidation;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.StoryShares.Commands.ShareStory;

public class ShareStoryCommandValidator : AbstractValidator<ShareStoryCommand>
{
    public ShareStoryCommandValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");

        RuleFor(x => x.Platform)
            .IsInEnum().WithMessage("Invalid share platform");
    }
}
