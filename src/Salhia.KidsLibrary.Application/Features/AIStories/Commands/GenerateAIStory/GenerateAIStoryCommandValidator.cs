using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;

public class GenerateAIStoryCommandValidator : AbstractValidator<GenerateAIStoryCommand>
{
    public GenerateAIStoryCommandValidator()
    {
        RuleFor(x => x.CustomStoryId)
            .NotEmpty().WithMessage("CustomStoryId is required")
            .Length(26).WithMessage("CustomStoryId must be a valid ULID (26 characters)");

        RuleFor(x => x.StoryName)
            .NotEmpty().WithMessage("StoryName is required")
            .MaximumLength(200).WithMessage("StoryName cannot exceed 200 characters");

        RuleFor(x => x.HeroName)
            .NotEmpty().WithMessage("HeroName is required")
            .MaximumLength(100).WithMessage("HeroName cannot exceed 100 characters");

        RuleFor(x => x.HeroImageUrl)
            .NotEmpty().WithMessage("HeroImageUrl is required")
            .Must(BeAValidUrl).WithMessage("HeroImageUrl must be a valid URL");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
