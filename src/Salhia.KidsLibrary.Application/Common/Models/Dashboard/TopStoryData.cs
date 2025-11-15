namespace Salhia.KidsLibrary.Application.Common.Models.Dashboard;

/// <summary>
/// Data container for top stories query results
/// </summary>
public class TopStoryData
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
    
    // Overall stats
    public int TotalViews { get; set; }
    public int LikesCount { get; set; }
    public int SharesCount { get; set; }
    public int RatingsCount { get; set; }
    public int RatingsSum { get; set; }
    
    // Period-specific viewer metrics
    public int UniqueViewers { get; set; }
    public int RepeatViewers { get; set; }
}
