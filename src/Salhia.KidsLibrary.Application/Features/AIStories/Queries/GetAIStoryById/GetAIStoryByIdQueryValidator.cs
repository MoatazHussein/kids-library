using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;

public class GetAIStoryByIdQueryValidator : AbstractValidator<GetAIStoryByIdQuery>
{
    public GetAIStoryByIdQueryValidator()
    {
        RuleFor(x => x.SlidesPageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1");

        RuleFor(x => x.SlidesPageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");
    }
}
