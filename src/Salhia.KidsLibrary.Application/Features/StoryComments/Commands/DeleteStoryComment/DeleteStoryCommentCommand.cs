using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.DeleteStoryComment;

public class DeleteStoryCommentCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
