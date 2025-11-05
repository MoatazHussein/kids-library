using MediatR;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Commands.AddStoryComment;

public class AddStoryCommentCommand : IRequest<string>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
