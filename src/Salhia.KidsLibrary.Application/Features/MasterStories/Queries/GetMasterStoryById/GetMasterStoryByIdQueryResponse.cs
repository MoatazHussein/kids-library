using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQueryResponse
{
    // Master Story Details
    public string Id { get; set; } = string.Empty;
    public string StoryCategoryId { get; set; } = string.Empty;
    public string StoryCategoryTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public MediaType MediaType { get; set; }
    public int MediaTypeValue => (int)MediaType;
    public string MediaTypeName => MediaType.ToString();
    public string MediaUrl { get; set; } = string.Empty;
    public int? PublishYear { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public UserInfoDto Author { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Paged Comments
    public PagedResult<GetStoryCommentsQueryResponse> Comments { get; set; } = null!;
    
    // Rating Statistics
    public int RatingsCount { get; set; }
    public decimal? AverageRating { get; set; } // Calculated: RatingsSum / RatingsCount
    
    // Like Statistics
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}
