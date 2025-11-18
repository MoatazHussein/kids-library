using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Commands.AddCustomStory;

public class AddCustomStoryCommand : IRequest<string>
{
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
