using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Commands.DeleteMediaItem;

public class DeleteMediaItemCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
