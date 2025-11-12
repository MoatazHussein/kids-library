using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

public class GetFavoriteStoriesQueryResponse
{
    // Favorite Info
    public string FavoriteId { get; set; } = string.Empty;
    public DateTime FavoritedAt { get; set; }
    
    // Master Story Details
    public string MasterStoryId { get; set; } = string.Empty;
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
    
    public string AuthorId { get; set; } = string.Empty;
    public UserInfoDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Counts
    public int MediaItemsCount { get; set; }
    public int CommentsCount { get; set; }
}
