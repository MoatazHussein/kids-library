namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Queries.GetRating;

public class GetRatingQueryResponse
{
    public string RatingId { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime RatedAt { get; set; }
}
