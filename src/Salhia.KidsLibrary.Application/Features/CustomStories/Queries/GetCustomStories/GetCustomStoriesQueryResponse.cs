using Salhia.KidsLibrary.Application.Common.Dtos.Users;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStories;

public class GetCustomStoriesQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public UserInfoDto? CreatedByUser { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public int CustomStoryItemsCount { get; set; }
}
