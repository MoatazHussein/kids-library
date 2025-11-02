using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.UpdateCustomStory;

public class UpdateCustomStoryCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
