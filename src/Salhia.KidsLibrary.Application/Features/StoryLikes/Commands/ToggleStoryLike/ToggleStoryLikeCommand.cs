using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryLikes.Commands.ToggleStoryLike;

public class ToggleStoryLikeCommand : IRequest<Unit>
{
    public string MasterStoryId { get; set; } = string.Empty;
}
