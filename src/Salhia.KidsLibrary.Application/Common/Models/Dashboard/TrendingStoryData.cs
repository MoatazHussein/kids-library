namespace Salhia.KidsLibrary.Application.Common.Models.Dashboard;

/// <summary>
/// Data container for trending stories query results
/// </summary>
public class TrendingStoryData
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
    
    // Recent period metrics
    public int RecentViews { get; set; }
    public int RecentLikes { get; set; }
    public int RecentShares { get; set; }
    public int NewViewers { get; set; }
    
    // Comparison period metrics
    public int ComparisonViews { get; set; }
}
