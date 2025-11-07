using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryRatings.Commands.DeleteRating;

public class DeleteRatingCommand : IRequest
{
    public string MasterStoryId { get; set; } = string.Empty;
}
