using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.DeleteCustomStory;

public class DeleteCustomStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
