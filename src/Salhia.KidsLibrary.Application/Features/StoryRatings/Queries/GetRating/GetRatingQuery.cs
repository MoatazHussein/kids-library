using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;

public class GetRatingQuery : IRequest<GetRatingQueryResponse?>
{
    public string MasterStoryId { get; set; } = string.Empty;
}
