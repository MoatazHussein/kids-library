using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.AddRating;

public class AddRatingCommand : IRequest<AddRatingCommandResponse>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5 stars
}
