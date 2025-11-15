namespace Salhia.KidsLibrary.Application.Common.Models.Dashboard;

/// <summary>
/// Data container for user statistics query results
/// </summary>
public class UserStatsData
{
    // User info
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    
    // Content stats
    public int StoriesCreated { get; set; }
    public int StoriesViewed { get; set; }
    public int UniqueStoriesViewed { get; set; }
    public int FavoriteStories { get; set; }
    
    // Engagement stats
    public int LikesGiven { get; set; }
    public int SharesGiven { get; set; }
    public int CommentsGiven { get; set; }
    public int RatingsGiven { get; set; }
    public int RatingsSum { get; set; }
    
    // Reading behavior
    public DateTime? LastActiveDate { get; set; }
    public List<DateTime> ActiveDates { get; set; } = []; // For streak calculation
    
    // Top categories
    public List<UserTopCategoryData> TopCategories { get; set; } = [];
    
    // Daily activity
    public List<UserDailyActivityData> DailyActivity { get; set; } = [];
}

public class UserTopCategoryData
{
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int LikesGiven { get; set; }
}

public class UserDailyActivityData
{
    public DateTime Date { get; set; }
    public int ViewsCount { get; set; }
    public int LikesGiven { get; set; }
    public int CommentsGiven { get; set; }
    public int SharesGiven { get; set; }
    public List<int> ViewHours { get; set; } = []; // Hours of day for time analysis
}
