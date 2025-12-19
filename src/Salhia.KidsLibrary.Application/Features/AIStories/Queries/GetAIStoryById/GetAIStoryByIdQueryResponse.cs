using Salhia.KidsLibrary.Application.Common.Dtos.AIStories;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;

public class GetAIStoryByIdQueryResponse
{
    // Story data
    public string Id { get; set; } = string.Empty;
    public string StoryName { get; set; } = string.Empty;
    public string HeroName { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public int SlidesCount { get; set; }
    public string CustomStoryId { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    public UserInfoDto? CreatedByUser { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Paged items (Slides)
    public PagedResult<AIStorySlideDto> Slides { get; set; } = null!;
}
