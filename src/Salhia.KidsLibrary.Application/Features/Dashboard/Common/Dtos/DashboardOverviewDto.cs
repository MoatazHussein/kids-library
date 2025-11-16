namespace Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

public class DashboardOverviewDto
{
    public StoryCountsDto StoryCounts { get; set; } = null!;
    public UserCountsDto UserCounts { get; set; } = null!;
    public EngagementMetricsDto EngagementMetrics { get; set; } = null!;
    public EngagementRatesDto EngagementRates { get; set; } = null!;
}

public class StoryCountsDto
{
    public int Total { get; set; }
    public int TotalInPeriod { get; set; }
    public int ApprovedInPeriod { get; set; }
    public int PendingInPeriod { get; set; }
    public int RejectedInPeriod { get; set; }
}

public class UserCountsDto
{
    public int Total { get; set; }
    public int ActiveUsersInPeriod { get; set; }
    public int ActiveInPeriod { get; set; }
    public int NewInPeriod { get; set; }
}

public class EngagementMetricsDto
{
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalShares { get; set; }
    public int TotalComments { get; set; }
    public int TotalRatings { get; set; }
    public decimal AverageRating { get; set; }
    
    public int ViewsInPeriod { get; set; }
    public int LikesInPeriod { get; set; }
    public int SharesInPeriod { get; set; }
    public int CommentsInPeriod { get; set; }
    public int RatingsInPeriod { get; set; }
}

public class EngagementRatesDto
{
    public decimal ViewToLikeRate { get; set; }
    public decimal ViewToShareRate { get; set; }
    public decimal ViewToCommentRate { get; set; }
    public decimal AverageEngagementScore { get; set; }
}
