using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQueryValidator : AbstractValidator<GetMediaItemsQuery>
{
    public GetMediaItemsQueryValidator()
    {
        RuleFor(x => x.MasterStoryId)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");

        RuleFor(x => x.PageNumber)
           .GreaterThanOrEqualTo(1)
           .WithMessage("Page number must be at least 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");

    }
}
