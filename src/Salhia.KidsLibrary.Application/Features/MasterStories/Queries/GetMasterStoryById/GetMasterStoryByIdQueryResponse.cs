using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQueryResponse
{
    // Master Story Details
    public string Id { get; set; } = string.Empty;
    public string StoryCategoryId { get; set; } = string.Empty;
    public string StoryCategoryTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsApproved { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public string? UpdatedByUserName { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Paged Media Items
    public PagedResult<GetMediaItemsQueryResponse> MediaItems { get; set; } = null!;
}
