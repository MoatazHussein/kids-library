using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.ApproveMasterStory;

public class ApproveMasterStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public bool IsApproved { get; set; }
}
