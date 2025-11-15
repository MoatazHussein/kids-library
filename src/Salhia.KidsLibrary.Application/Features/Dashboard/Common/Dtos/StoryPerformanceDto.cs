namespace Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

public class StoryPerformanceDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
    
    // Core engagement metrics
    public int TotalViews { get; set; }
    public int UniqueViewers { get; set; }
    public int LikesCount { get; set; }
    public int SharesCount { get; set; }
    public int RatingsCount { get; set; }
    public decimal? AverageRating { get; set; }
    
    // Calculated metrics
    public decimal EngagementScore { get; set; }
    public decimal RepeatViewerRate { get; set; }
}
