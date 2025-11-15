using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Models.Dashboard;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetUserStats;

public class GetUserStatsQueryHandler(
    IDashboardRepository dashboardRepository,
    ITimeZoneConverter timeZoneConverter,
    ILogger<GetUserStatsQueryHandler> logger
) : IRequestHandler<GetUserStatsQuery, GetUserStatsQueryResponse>
{
    public async Task<GetUserStatsQueryResponse> Handle(
        GetUserStatsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting stats for user {UserId} with period {Period}",
            request.UserId, request.Period);

        // Calculate period date
        var periodFrom = GetPeriodStartDate(request.Period);

        // Get user stats from repository (optimized queries)
        var userStatsData = await dashboardRepository.GetUserStatsAsync(
            request.UserId,
            periodFrom,
            request.IncludeTopCategories,
            request.IncludeDailyActivity,
            request.TopCategoriesLimit,
            cancellationToken);

        // Calculate derived metrics
        var avgRatingGiven = userStatsData.RatingsGiven > 0
            ? Math.Round((decimal)userStatsData.RatingsSum / userStatsData.RatingsGiven, 2)
            : 0;

        var readingStreak = CalculateReadingStreak(userStatsData.ActiveDates);
        var mostActiveTimeOfDay = CalculateMostActiveTimeOfDay(userStatsData.DailyActivity);

        // Map to DTO
        var stats = new UserStatsDto
        {
            UserId = userStatsData.UserId,
            Username = userStatsData.Username,
            Email = userStatsData.Email,
            StoriesCreated = userStatsData.StoriesCreated,
            StoriesViewed = userStatsData.StoriesViewed,
            UniqueStoriesViewed = userStatsData.UniqueStoriesViewed,
            FavoriteStories = userStatsData.FavoriteStories,
            LikesGiven = userStatsData.LikesGiven,
            SharesGiven = userStatsData.SharesGiven,
            CommentsGiven = userStatsData.CommentsGiven,
            RatingsGiven = userStatsData.RatingsGiven,
            AverageRatingGiven = avgRatingGiven,
            ReadingStreakDays = readingStreak,
            LastActiveDate = userStatsData.LastActiveDate,
            MostActiveTimeOfDay = mostActiveTimeOfDay,
            TopCategories = userStatsData.TopCategories.Select(tc => new UserTopCategoryDto
            {
                CategoryId = tc.CategoryId,
                CategoryName = tc.CategoryName,
                ViewCount = tc.ViewCount,
                LikesGiven = tc.LikesGiven
            }).ToList(),
            DailyActivity = userStatsData.DailyActivity.Select(da => new UserDailyActivityDto
            {
                Date = da.Date,
                ViewsCount = da.ViewsCount,
                LikesGiven = da.LikesGiven,
                CommentsGiven = da.CommentsGiven,
                SharesGiven = da.SharesGiven
            }).ToList()
        };

        var response = new GetUserStatsQueryResponse
        {
            Stats = stats
        };

        return timeZoneConverter.ConvertUtcToLocal(response);
    }

    private static DateTime GetPeriodStartDate(DashboardPeriod period)
    {
        var now = DateTime.UtcNow;
        return period switch
        {
            DashboardPeriod.Last24Hours => now.AddDays(-1),
            DashboardPeriod.Last7Days => now.AddDays(-7),
            DashboardPeriod.Last30Days => now.AddDays(-30),
            DashboardPeriod.Last90Days => now.AddDays(-90),
            _ => now.AddDays(-30)
        };
    }

    private static int CalculateReadingStreak(List<DateTime> activeDates)
    {
        if (activeDates.Count == 0) return 0;

        var today = DateTime.UtcNow.Date;
        var streak = 0;

        // Check if user was active today or yesterday (streak continues)
        if (!activeDates.Contains(today) && !activeDates.Contains(today.AddDays(-1)))
        {
            return 0; // Streak is broken
        }

        // Count consecutive days backward from today
        var currentDate = activeDates.Contains(today) ? today : today.AddDays(-1);
        
        while (activeDates.Contains(currentDate))
        {
            streak++;
            currentDate = currentDate.AddDays(-1);
        }

        return streak;
    }

    private static string? CalculateMostActiveTimeOfDay(List<UserDailyActivityData> dailyActivity)
    {
        if (dailyActivity.Count == 0) return null;

        // Collect all view hours
        var allHours = dailyActivity.SelectMany(da => da.ViewHours).ToList();
        if (allHours.Count == 0) return null;

        // Group hours into time periods
        var morning = allHours.Count(h => h >= 6 && h < 12);   // 6 AM - 12 PM
        var afternoon = allHours.Count(h => h >= 12 && h < 18); // 12 PM - 6 PM
        var evening = allHours.Count(h => h >= 18 && h < 22);   // 6 PM - 10 PM
        var night = allHours.Count(h => h >= 22 || h < 6);      // 10 PM - 6 AM

        var max = Math.Max(Math.Max(morning, afternoon), Math.Max(evening, night));

        if (max == morning) return "Morning";
        if (max == afternoon) return "Afternoon";
        if (max == evening) return "Evening";
        return "Night";
    }
}
