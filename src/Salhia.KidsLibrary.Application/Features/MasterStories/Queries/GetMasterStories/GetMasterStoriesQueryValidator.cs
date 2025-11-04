using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQueryValidator : AbstractValidator<GetMasterStoriesQuery>
{
    public GetMasterStoriesQueryValidator()
    {
        RuleFor(x => x.StoryCategoryId)
            .Length(26).WithMessage("Story Category ID must be 26 characters (ULID format)")
            .When(x => !string.IsNullOrWhiteSpace(x.StoryCategoryId));

        RuleFor(x => x.PageNumber)
           .GreaterThanOrEqualTo(1)
           .WithMessage("Page number must be at least 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");
    }
}
