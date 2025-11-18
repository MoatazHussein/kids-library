using Salhia.KidsLibrary.Application.Common.Dtos.CustomStories;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;

public class GetCustomStoryByIdQueryResponse
{
    // Story data
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public UserInfoDto? CreatedByUser { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Paged items
    public PagedResult<CustomStoryItemDto> Items { get; set; } = null!;
}
