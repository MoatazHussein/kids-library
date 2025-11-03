using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;

public class GetCustomStoryByIdQueryValidator : AbstractValidator<GetCustomStoryByIdQuery>
{
    public GetCustomStoryByIdQueryValidator()
    {
        RuleFor(x => x.ItemsPageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1");

        RuleFor(x => x.ItemsPageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");
    }
}
