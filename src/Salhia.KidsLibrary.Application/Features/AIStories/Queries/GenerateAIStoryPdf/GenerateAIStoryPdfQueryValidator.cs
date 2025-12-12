using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GenerateAIStoryPdf;

public class GenerateAIStoryPdfQueryValidator : AbstractValidator<GenerateAIStoryPdfQuery>
{
    public GenerateAIStoryPdfQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("AI Story ID is required")
            .Length(26).WithMessage("AI Story ID must be 26 characters (ULID format)");
    }
}
