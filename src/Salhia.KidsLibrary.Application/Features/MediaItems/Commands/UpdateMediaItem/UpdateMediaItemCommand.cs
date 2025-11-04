using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.UpdateMediaItem;

public class UpdateMediaItemCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
}
