using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GenerateCustomStoryPdf;

public class GenerateCustomStoryPdfQueryValidator : AbstractValidator<GenerateCustomStoryPdfQuery>
{
    public GenerateCustomStoryPdfQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Custom Story ID is required")
            .Length(26).WithMessage("Custom Story ID must be 26 characters (ULID format)");
    }
}
