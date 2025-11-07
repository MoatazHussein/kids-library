namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.UpdateRating;

public class UpdateRatingCommandResponse
{
    public string RatingId { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public int NewRating { get; set; }
    public int PreviousRating { get; set; }
}
