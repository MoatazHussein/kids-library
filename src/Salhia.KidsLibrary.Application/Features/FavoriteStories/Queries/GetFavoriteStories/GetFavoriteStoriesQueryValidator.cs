using FluentValidation;
using Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

public class GetFavoriteStoriesQueryValidator : AbstractValidator<GetFavoriteStoriesQuery>
{
    public GetFavoriteStoriesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");
    }
}
