using FluentValidation;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQueryValidator : AbstractValidator<GetMasterStoryByIdQuery>
{
    public GetMasterStoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Master Story ID is required")
            .Length(26).WithMessage("Master Story ID must be 26 characters (ULID format)");

        RuleFor(x => x.CommentsPageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Comments page number must be at least 1");

        RuleFor(x => x.CommentsPageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Comments page size must be at least 1");
    }
}
