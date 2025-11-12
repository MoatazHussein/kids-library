using MediatR;
using Salhia.KidsLibrary.Application.Common.Dtos.MediaItems;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Commands.AddMasterStoryWithItems;

public class AddMasterStoryWithItemsCommand : IRequest<string>
{
    public string StoryCategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    
    // Items to create with the story
    public List<MediaItemDto> MediaItems { get; set; } = [];
}
