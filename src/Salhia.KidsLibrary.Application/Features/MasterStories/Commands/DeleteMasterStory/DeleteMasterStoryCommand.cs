using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.DeleteMasterStory;

public class DeleteMasterStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
