namespace Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

public class TrendingStoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
    
    // Trending metrics
    public int RecentViews { get; set; }
    public int RecentLikes { get; set; }
    public int RecentShares { get; set; }
    public int TrendingScore { get; set; }
    
    // Growth indicators
    public decimal ViewsGrowthRate { get; set; }  // Percentage
    public int NewViewers { get; set; }
}
