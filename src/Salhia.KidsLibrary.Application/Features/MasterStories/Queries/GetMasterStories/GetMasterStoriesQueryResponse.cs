using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
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
    public UserInfoDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public UserInfoDto? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Counts
    public int CommentsCount { get; set; }
    
    // Rating Statistics
    public int RatingsCount { get; set; }
    public decimal? AverageRating { get; set; } // Calculated: RatingsSum / RatingsCount
    
    // Like Statistics
    public int LikesCount { get; set; }
    
    // Share Statistics
    public int SharesCount { get; set; }
    
    // View Statistics
    public int TotalViews { get; set; }
}
