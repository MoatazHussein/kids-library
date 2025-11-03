using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;

public class GetStoryCategoriesQueryValidator : AbstractValidator<GetStoryCategoriesQuery>
{
    public GetStoryCategoriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");
    }
}
