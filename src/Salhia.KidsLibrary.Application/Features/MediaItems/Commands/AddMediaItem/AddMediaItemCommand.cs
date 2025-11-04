using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.AddMediaItem;

public class AddMediaItemCommand : IRequest<string>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
}
