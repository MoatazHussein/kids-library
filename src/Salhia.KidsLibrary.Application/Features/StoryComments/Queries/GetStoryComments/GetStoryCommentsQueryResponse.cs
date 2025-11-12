using Salhia.KidsLibrary.Application.Common.Dtos.Users;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;

public class GetStoryCommentsQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    public UserInfoDto? CreatedByUser { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
