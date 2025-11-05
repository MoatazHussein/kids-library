using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.UpdateStoryComment;

public class UpdateStoryCommentCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
