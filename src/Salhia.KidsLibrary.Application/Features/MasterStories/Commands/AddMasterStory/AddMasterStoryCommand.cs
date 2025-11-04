using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStory;

public class AddMasterStoryCommand : IRequest<string>
{
    public string StoryCategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
}
