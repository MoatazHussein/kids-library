using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models.Dashboard;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Infrastructure.Persistence;

namespace Salhia.KidsLibrary.Infrastructure.Repositories;

public class DashboardRepository(AppDbContext context, ILogger<DashboardRepository> logger) : IDashboardRepository
{
    public async Task<List<TrendingStoryData>> GetTrendingStoriesAsync(
        DateTime recentFrom,
        DateTime comparisonFrom,
        int top,
        string? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing optimized trending stories query");

        var query = from ms in context.MasterStories
                    where (categoryId == null || ms.StoryCategoryId == categoryId) && ms.ApprovalStatus == ApprovalStatus.Approved
                    select new TrendingStoryData
                    {
                        Id = ms.Id,
                        Title = ms.Title,
                        CoverImageUrl = ms.CoverImageUrl,
                        StoryCategoryId = ms.StoryCategoryId,
                        StoryCategoryTitle = ms.StoryCategory!.Title,
                        
                        // Recent period metrics
                        RecentViews = ms.ViewSessions
                            .Where(vs => vs.LastViewAt >= recentFrom)
                            .Sum(vs => vs.ViewCount),
                        
                        RecentLikes = ms.Likes
                            .Count(l => l.CreatedAt >= recentFrom),
                        
                        RecentShares = ms.Shares
                            .Count(s => s.CreatedAt >= recentFrom),
                        
                        NewViewers = ms.ViewSessions
                            .Count(vs => vs.CreatedAt >= recentFrom),
                        
                        // Comparison period metrics
                        ComparisonViews = ms.ViewSessions
                            .Where(vs => vs.LastViewAt >= comparisonFrom && vs.LastViewAt < recentFrom)
                            .Sum(vs => vs.ViewCount)
                    };

        // Filter to stories with recent activity and take top N
        var results = await query
            .Where(t => t.RecentViews > 0)
            .OrderByDescending(t => t.RecentViews)
            .Take(top * 2) // Take more initially for in-memory trending score calculation
            .ToListAsync(cancellationToken);

        return results;
    }

    public async Task<List<TopStoryData>> GetTopStoriesAsync(
        DateTime? periodFrom,
        SortBy sortBy,
        int top,
        string? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing optimized top stories query");

        var query = from ms in context.MasterStories
                    where (categoryId == null || ms.StoryCategoryId == categoryId ) && ms.ApprovalStatus == ApprovalStatus.Approved
                    join stats in context.MasterStoryStats on ms.Id equals stats.MasterStoryId into statsGroup
                    from stats in statsGroup.DefaultIfEmpty()
                    select new TopStoryData
                    {
                        Id = ms.Id,
                        Title = ms.Title,
                        CoverImageUrl = ms.CoverImageUrl,
                        StoryCategoryId = ms.StoryCategoryId,
                        StoryCategoryTitle = ms.StoryCategory!.Title,
                        
                        // Overall stats from cache
                        TotalViews = stats != null ? stats.TotalViews : 0,
                        LikesCount = stats != null ? stats.LikesCount : 0,
                        SharesCount = stats != null ? stats.SharesCount : 0,
                        RatingsCount = stats != null ? stats.RatingsCount : 0,
                        RatingsSum = stats != null ? stats.RatingsSum : 0,
                        
                        // Period-specific viewer metrics (if period specified, otherwise use stats)
                        UniqueViewers = periodFrom.HasValue
                            ? ms.ViewSessions.Count(vs => vs.LastViewAt >= periodFrom.Value)
                            : (stats != null ? stats.TotalViews : 0),
                        
                        RepeatViewers = periodFrom.HasValue
                            ? ms.ViewSessions.Count(vs => vs.LastViewAt >= periodFrom.Value && vs.ViewCount > 1)
                            : 0
                    };

        // Apply sorting based on sortBy parameter (done in-memory after fetch for complex calculations)
        var results = await query.ToListAsync(cancellationToken);

        return results;
    }

    public async Task<StoryEngagementData> GetStoryEngagementAsync(
        string storyId,
        DateTime periodFrom,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing optimized story engagement query for story {StoryId}", storyId);

        // Main story data with period metrics
        var storyData = await (
            from ms in context.MasterStories
            where ms.Id == storyId && ms.ApprovalStatus == ApprovalStatus.Approved
            join stats in context.MasterStoryStats on ms.Id equals stats.MasterStoryId into statsGroup
            from stats in statsGroup.DefaultIfEmpty()
            select new StoryEngagementData
            {
                Id = ms.Id,
                Title = ms.Title,
                CoverImageUrl = ms.CoverImageUrl,
                
                // Overall metrics from stats cache
                TotalViews = stats != null ? stats.TotalViews : 0,
                TotalLikes = stats != null ? stats.LikesCount : 0,
                TotalShares = stats != null ? stats.SharesCount : 0,
                TotalComments = ms.Comments.Count(),
                TotalRatings = stats != null ? stats.RatingsCount : 0,
                RatingsSum = stats != null ? stats.RatingsSum : 0,
                
                // Period-specific metrics
                PeriodViews = ms.ViewSessions
                    .Where(vs => vs.LastViewAt >= periodFrom)
                    .Sum(vs => vs.ViewCount),
                
                PeriodLikes = ms.Likes
                    .Count(l => l.CreatedAt >= periodFrom),
                
                PeriodShares = ms.Shares
                    .Count(s => s.CreatedAt >= periodFrom),
                
                PeriodComments = ms.Comments
                    .Count(c => c.CreatedAt >= periodFrom),
                
                PeriodRatings = ms.Ratings
                    .Count(r => r.CreatedAt >= periodFrom),
                
                // Viewer insights
                UniqueViewers = ms.ViewSessions
                    .Count(vs => vs.LastViewAt >= periodFrom),
                
                ReturningViewers = ms.ViewSessions
                    .Count(vs => vs.LastViewAt >= periodFrom && vs.ViewCount > 1)
            }).FirstOrDefaultAsync(cancellationToken);

        if (storyData == null)
        {
            throw new KeyNotFoundException($"Story with ID {storyId} not found");
        }

        // Get daily breakdown (separate query for better performance)
        var days = (int)(DateTime.UtcNow - periodFrom).TotalDays;
        var dailyBreakdown = new List<DailyEngagementData>();

        for (int i = days; i >= 0; i--)
        {
            var date = DateTime.UtcNow.Date.AddDays(-i);
            var nextDate = date.AddDays(1);

            var dayData = await (
                from ms in context.MasterStories
                where ms.Id == storyId && ms.ApprovalStatus == ApprovalStatus.Approved
                select new DailyEngagementData
                {
                    Date = date,
                    Views = ms.ViewSessions
                        .Where(vs => vs.LastViewAt >= date && vs.LastViewAt < nextDate)
                        .Sum(vs => vs.ViewCount),
                    
                    Likes = ms.Likes
                        .Count(l => l.CreatedAt >= date && l.CreatedAt < nextDate),
                    
                    Shares = ms.Shares
                        .Count(s => s.CreatedAt >= date && s.CreatedAt < nextDate),
                    
                    Comments = ms.Comments
                        .Count(c => c.CreatedAt >= date && c.CreatedAt < nextDate)
                }).FirstOrDefaultAsync(cancellationToken);

            if (dayData != null)
            {
                dailyBreakdown.Add(dayData);
            }
        }

        storyData.DailyBreakdown = dailyBreakdown;

        return storyData;
    }

    public async Task<UserStatsData> GetUserStatsAsync(
        string userId,
        DateTime periodFrom,
        bool includeTopCategories = true,
        bool includeDailyActivity = true,
        int topCategoriesLimit = 5,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing optimized user stats query for user {UserId}", userId);

        // Get user info
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found");
        }

        // Main user stats query
        var userStats = new UserStatsData
        {
            UserId = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email,
            
            // Content stats (all time)
            StoriesCreated = await context.CustomStories
                .CountAsync(cs => cs.CreatedBy == userId, cancellationToken),
            
            FavoriteStories = await context.FavoriteStories
                .CountAsync(fs => fs.UserId == userId, cancellationToken),
            
            // Period-specific viewing
            StoriesViewed = await context.StoryViewSessions
                .CountAsync(vs => vs.UserId == userId && vs.LastViewAt >= periodFrom, cancellationToken),
            
            UniqueStoriesViewed = await context.StoryViewSessions
                .Where(vs => vs.UserId == userId && vs.LastViewAt >= periodFrom)
                .Select(vs => vs.MasterStoryId)
                .Distinct()
                .CountAsync(cancellationToken),
            
            // Engagement stats (period-specific)
            LikesGiven = await context.StoryLikes
                .CountAsync(l => l.UserId == userId && l.CreatedAt >= periodFrom, cancellationToken),
            
            SharesGiven = await context.StoryShares
                .CountAsync(s => s.UserId == userId && s.CreatedAt >= periodFrom, cancellationToken),
            
            CommentsGiven = await context.StoryComments
                .CountAsync(c => c.CreatedBy == userId && c.CreatedAt >= periodFrom, cancellationToken),
            
            RatingsGiven = await context.StoryRatings
                .CountAsync(r => r.UserId == userId && r.CreatedAt >= periodFrom, cancellationToken),
            
            RatingsSum = await context.StoryRatings
                .Where(r => r.UserId == userId && r.CreatedAt >= periodFrom)
                .SumAsync(r => (int)r.Rating, cancellationToken),
            
            // Last active
            LastActiveDate = await context.StoryViewSessions
                .Where(vs => vs.UserId == userId)
                .OrderByDescending(vs => vs.LastViewAt)
                .Select(vs => (DateTime?)vs.LastViewAt)
                .FirstOrDefaultAsync(cancellationToken),
            
            // Active dates for streak calculation
            ActiveDates = await context.StoryViewSessions
                .Where(vs => vs.UserId == userId && vs.LastViewAt >= periodFrom)
                .Select(vs => vs.LastViewAt.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync(cancellationToken)
        };

        // Get top categories if requested
        if (includeTopCategories)
        {
            userStats.TopCategories = await (
                from vs in context.StoryViewSessions
                where vs.UserId == userId && vs.LastViewAt >= periodFrom 
                join ms in context.MasterStories on vs.MasterStoryId equals ms.Id 
                join sc in context.StoryCategories on ms.StoryCategoryId equals sc.Id
                group new { vs, ms } by new { sc.Id, sc.Title } into g
                orderby g.Count() descending
                select new UserTopCategoryData
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Title,
                    ViewCount = g.Count(),
                    LikesGiven = context.StoryLikes
                        .Count(l => l.UserId == userId && 
                                   l.MasterStory.StoryCategoryId == g.Key.Id && 
                                   l.CreatedAt >= periodFrom)
                })
                .Take(topCategoriesLimit)
                .ToListAsync(cancellationToken);
        }

        // Get daily activity if requested
        if (includeDailyActivity)
        {
            var days = (int)(DateTime.UtcNow - periodFrom).TotalDays;
            var dailyActivity = new List<UserDailyActivityData>();

            for (int i = days; i >= 0; i--)
            {
                var date = DateTime.UtcNow.Date.AddDays(-i);
                var nextDate = date.AddDays(1);

                var dayActivity = new UserDailyActivityData
                {
                    Date = date,
                    ViewsCount = await context.StoryViewSessions
                        .CountAsync(vs => vs.UserId == userId && 
                                    vs.LastViewAt >= date && 
                                    vs.LastViewAt < nextDate, cancellationToken),
                    
                    LikesGiven = await context.StoryLikes
                        .CountAsync(l => l.UserId == userId && 
                                   l.CreatedAt >= date && 
                                   l.CreatedAt < nextDate, cancellationToken),
                    
                    CommentsGiven = await context.StoryComments
                        .CountAsync(c => c.CreatedBy == userId && 
                                   c.CreatedAt >= date && 
                                   c.CreatedAt < nextDate, cancellationToken),
                    
                    SharesGiven = await context.StoryShares
                        .CountAsync(s => s.UserId == userId && 
                                   s.CreatedAt >= date && 
                                   s.CreatedAt < nextDate, cancellationToken),
                    
                    ViewHours = await context.StoryViewSessions
                        .Where(vs => vs.UserId == userId && 
                                    vs.LastViewAt >= date && 
                                    vs.LastViewAt < nextDate)
                        .Select(vs => vs.LastViewAt.Hour)
                        .ToListAsync(cancellationToken)
                };

                dailyActivity.Add(dayActivity);
            }

            userStats.DailyActivity = dailyActivity;
        }

        return userStats;
    }

    public async Task<DashboardOverviewData> GetDashboardOverviewAsync(
        DashboardPeriod period,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing optimized dashboard overview queries for period {Period}", period);

        var periodFrom = GetPeriodStartDate(period);
        

        // Query 1: Counts query
        var totalStories = await context.MasterStories.CountAsync(cancellationToken);
        var totalStoriesInPeriod = await context.MasterStories.CountAsync(s => s.CreatedAt >= periodFrom, cancellationToken);
        var approvedStoriesInPeriod = await context.MasterStories.CountAsync(
            s => s.ApprovalStatus == ApprovalStatus.Approved  && s.CreatedAt >= periodFrom, cancellationToken);
        var pendingStoriesInPeriod = await context.MasterStories.CountAsync(
            s => s.ApprovalStatus == ApprovalStatus.Pending && s.CreatedAt >= periodFrom, cancellationToken);
        var rejectedStoriesInPeriod = await context.MasterStories.CountAsync(
            s => s.ApprovalStatus == ApprovalStatus.Rejected && s.CreatedAt >= periodFrom, cancellationToken);

        var totalUsers = await context.Users.CountAsync(cancellationToken);
        var activeUsersInPeriod = await context.StoryViewSessions
            .Where(vs => vs.LastViewAt >= periodFrom)
            .Select(vs => vs.UserId)
            .Distinct()
            .CountAsync(cancellationToken);
        var newUsersInPeriod = await context.Users.CountAsync(u => u.CreatedAt >= periodFrom, cancellationToken);

        // Query 2: Period engagement (only on approved stories)
        var viewsInPeriod = await context.StoryViewSessions
            .Where(vs => vs.LastViewAt >= periodFrom && 
                   context.MasterStories.Any(ms => ms.Id == vs.MasterStoryId))
            .SumAsync(vs => vs.ViewCount, cancellationToken);

        var likesInPeriod = await context.StoryLikes
            .Where(l => l.CreatedAt >= periodFrom &&
                   context.MasterStories.Any(ms => ms.Id == l.MasterStoryId))
            .CountAsync(cancellationToken);

        var sharesInPeriod = await context.StoryShares
            .Where(s => s.CreatedAt >= periodFrom &&
                   context.MasterStories.Any(ms => ms.Id == s.MasterStoryId))
            .CountAsync(cancellationToken);

        var commentsInPeriod = await context.StoryComments
            .Where(c => c.CreatedAt >= periodFrom &&
                   context.MasterStories.Any(ms => ms.Id == c.MasterStoryId))
            .CountAsync(cancellationToken);

        var ratingsInPeriod = await context.StoryRatings
            .Where(r => r.CreatedAt >= periodFrom &&
                   context.MasterStories.Any(ms => ms.Id == r.MasterStoryId))
            .CountAsync(cancellationToken);

        // Query 3: Stats cache query (use MasterStoryStats for instant totals)
        var totalStats = await context.MasterStoryStats
            .GroupBy(s => 1)
            .Select(g => new
            {
                TotalViews = g.Sum(s => (int?)s.TotalViews) ?? 0,
                TotalLikes = g.Sum(s => (int?)s.LikesCount) ?? 0,
                TotalShares = g.Sum(s => (int?)s.SharesCount) ?? 0,
                TotalComments = context.StoryComments.Count(),
                TotalRatings = g.Sum(s => (int?)s.RatingsCount) ?? 0,
                TotalRatingsSum = g.Sum(s => (int?)s.RatingsSum) ?? 0
            }).FirstOrDefaultAsync(cancellationToken);

        // Calculate average rating
        var averageRating = totalStats?.TotalRatings > 0
            ? (decimal)totalStats.TotalRatingsSum / totalStats.TotalRatings
            : 0;

        return new DashboardOverviewData
        {
            // Story counts
            TotalStories = totalStories,
            TotalStoriesInPeriod = totalStoriesInPeriod,
            ApprovedStoriesInPeriod = approvedStoriesInPeriod,
            PendingStoriesInPeriod = pendingStoriesInPeriod,
            RejectedStoriesInPeriod = rejectedStoriesInPeriod,
            
            // User counts
            TotalUsers = totalUsers,
            ActiveUsersInPeriod = activeUsersInPeriod,
            NewUsersInPeriod = newUsersInPeriod,
            
            // Period engagement
            ViewsInPeriod = viewsInPeriod,
            LikesInPeriod = likesInPeriod,
            SharesInPeriod = sharesInPeriod,
            CommentsInPeriod = commentsInPeriod,
            RatingsInPeriod = ratingsInPeriod,
            
            // Total engagement
            TotalViews = totalStats?.TotalViews ?? 0,
            TotalLikes = totalStats?.TotalLikes ?? 0,
            TotalShares = totalStats?.TotalShares ?? 0,
            TotalComments = totalStats?.TotalComments ?? 0,
            TotalRatings = totalStats?.TotalRatings ?? 0,
            TotalAverageRating = Math.Round(averageRating, 2)
        };
    }

    private static DateTime GetPeriodStartDate(DashboardPeriod period)
    {
        return period switch
        {
            DashboardPeriod.Last24Hours => DateTime.UtcNow.AddHours(-24),
            DashboardPeriod.Last7Days => DateTime.UtcNow.AddDays(-7),
            DashboardPeriod.Last30Days => DateTime.UtcNow.AddDays(-30),
            DashboardPeriod.Last90Days => DateTime.UtcNow.AddDays(-90),
            DashboardPeriod.AllTime => DateTime.MinValue,
            _ => DateTime.UtcNow.AddDays(-30)
        };
    }
}
