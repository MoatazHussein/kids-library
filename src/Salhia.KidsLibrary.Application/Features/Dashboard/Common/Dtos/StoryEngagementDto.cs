namespace Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

public class StoryEngagementDto
{
    // Story info
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    
    // Overall metrics
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalShares { get; set; }
    public int TotalComments { get; set; }
    public int TotalRatings { get; set; }
    public decimal AverageRating { get; set; }
    
    // Period-specific metrics
    public int PeriodViews { get; set; }
    public int PeriodLikes { get; set; }
    public int PeriodShares { get; set; }
    public int PeriodComments { get; set; }
    public int PeriodRatings { get; set; }
    
    // Engagement rates (period-based)
    public decimal LikeRate { get; set; }  // Likes per view
    public decimal ShareRate { get; set; }  // Shares per view
    public decimal CommentRate { get; set; }  // Comments per view
    
    // Viewer insights
    public int UniqueViewers { get; set; }
    public int ReturningViewers { get; set; }
    public decimal ReturnRate { get; set; }  // Percentage
    public decimal AverageViewsPerVisitor { get; set; }
    
    // Time-based insights
    public List<DailyEngagementDto> DailyBreakdown { get; set; } = [];
}

public class DailyEngagementDto
{
    public DateTime Date { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Shares { get; set; }
    public int Comments { get; set; }
}
