namespace Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

public class UserStatsDto
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
    public decimal AverageRatingGiven { get; set; }
    
    // Reading behavior
    public int ReadingStreakDays { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public string? MostActiveTimeOfDay { get; set; } // Morning, Afternoon, Evening, Night
    
    // Top categories
    public List<UserTopCategoryDto> TopCategories { get; set; } = [];
    
    // Activity timeline
    public List<UserDailyActivityDto> DailyActivity { get; set; } = [];
}

public class UserTopCategoryDto
{
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public int LikesGiven { get; set; }
}

public class UserDailyActivityDto
{
    public DateTime Date { get; set; }
    public int ViewsCount { get; set; }
    public int LikesGiven { get; set; }
    public int CommentsGiven { get; set; }
    public int SharesGiven { get; set; }
}
