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
    public string? ImageUrl { get; set; }
    public bool IsApproved { get; set; }
    
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Counts
    public int MediaItemsCount { get; set; }
    public int CommentsCount { get; set; }
}
