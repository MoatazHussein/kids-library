namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.AddRating;

public class AddRatingCommandResponse
{
    public string RatingId { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public int Rating { get; set; }
}
