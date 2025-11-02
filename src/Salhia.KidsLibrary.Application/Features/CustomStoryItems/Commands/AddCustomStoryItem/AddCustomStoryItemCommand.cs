using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStoryItems.Commands.AddCustomStoryItem;

public class AddCustomStoryItemCommand : IRequest<string>
{
    public string CustomStoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
