using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.UpdateRating;

public class UpdateRatingCommand : IRequest<UpdateRatingCommandResponse>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5 stars
}
