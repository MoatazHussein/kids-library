namespace Salhia.KidsLibrary.Application.Common.Models.Dashboard;

public class DashboardOverviewData
{
    // Story Counts
    public int TotalStories { get; set; }
    public int TotalStoriesInPeriod { get; set; }
    public int ApprovedStoriesInPeriod { get; set; }
    public int PendingStoriesInPeriod { get; set; }
    public int RejectedStoriesInPeriod { get; set; }
    
    // User Counts
    public int TotalUsers { get; set; }
    public int ActiveUsersInPeriod { get; set; }
    public int NewUsersInPeriod { get; set; }
    
    // Period Engagement
    public int ViewsInPeriod { get; set; }
    public int LikesInPeriod { get; set; }
    public int SharesInPeriod { get; set; }
    public int CommentsInPeriod { get; set; }
    public int RatingsInPeriod { get; set; }
    
    // Total Engagement (All Time)
    public int TotalViews { get; set; }
    public int TotalLikes { get; set; }
    public int TotalShares { get; set; }
    public int TotalComments { get; set; }
    public int TotalRatings { get; set; }
    public decimal TotalAverageRating { get; set; }
}
