namespace Salhia.KidsLibrary.Application.Common.Models.Dashboard;

/// <summary>
/// Data container for story engagement query results
/// </summary>
public class StoryEngagementData
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
    public int RatingsSum { get; set; }
    
    // Period-specific metrics
    public int PeriodViews { get; set; }
    public int PeriodLikes { get; set; }
    public int PeriodShares { get; set; }
    public int PeriodComments { get; set; }
    public int PeriodRatings { get; set; }
    
    // Viewer insights
    public int UniqueViewers { get; set; }
    public int ReturningViewers { get; set; }
    
    // Daily breakdown
    public List<DailyEngagementData> DailyBreakdown { get; set; } = [];
}

/// <summary>
/// Data container for daily engagement breakdown
/// </summary>
public class DailyEngagementData
{
    public DateTime Date { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Shares { get; set; }
    public int Comments { get; set; }
}
