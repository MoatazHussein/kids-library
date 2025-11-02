using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.UpdateCustomStoryItem;

public class UpdateCustomStoryItemCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
