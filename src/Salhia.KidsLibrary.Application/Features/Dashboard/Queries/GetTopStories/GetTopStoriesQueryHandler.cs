using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTopStories;

public class GetTopStoriesQueryHandler(
    IDashboardRepository dashboardRepository,
    ITimeZoneConverter timeZoneConverter,
    ILogger<GetTopStoriesQueryHandler> logger
) : IRequestHandler<GetTopStoriesQuery, GetTopStoriesQueryResponse>
{
    public async Task<GetTopStoriesQueryResponse> Handle(
        GetTopStoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting top {Top} stories sorted by {SortBy} for period {Period}",
            request.Top, request.SortBy, request.Period);

        // Calculate period date
        DateTime? fromDate = request.Period switch
        {
            DashboardPeriod.Last24Hours => DateTime.UtcNow.AddHours(-24),
            DashboardPeriod.Last7Days => DateTime.UtcNow.AddDays(-7),
            DashboardPeriod.Last30Days => DateTime.UtcNow.AddDays(-30),
            DashboardPeriod.Last90Days => DateTime.UtcNow.AddDays(-90),
            DashboardPeriod.AllTime => null,
            _ => null
        };

        // Get top stories data from repository (optimized single query)
        var topStoriesData = await dashboardRepository.GetTopStoriesAsync(
            fromDate,
            request.SortBy,
            request.Top * 2, // Get more for sorting flexibility
            request.CategoryId,
            cancellationToken);

        // Map to DTOs with calculated metrics
        var performanceList = topStoriesData.Select(data =>
        {
            var uniqueViewers = data.UniqueViewers;
            var totalViews = request.Period == DashboardPeriod.AllTime
                ? data.TotalViews
                : data.UniqueViewers; // For periods, use session-based unique viewers

            var avgRating = data.RatingsCount > 0
                ? Math.Round((decimal)data.RatingsSum / data.RatingsCount, 2)
                : (decimal?)null;

            var engagementScore = CalculateEngagementScore(totalViews, data.LikesCount, data.SharesCount, data.RatingsCount);
            
            var repeatViewerRate = uniqueViewers > 0
                ? Math.Round((decimal)data.RepeatViewers / uniqueViewers * 100, 2)
                : 0;

            return new StoryPerformanceDto
            {
                Id = data.Id,
                Title = data.Title,
                CoverImageUrl = data.CoverImageUrl,
                StoryCategoryId = data.StoryCategoryId,
                StoryCategoryTitle = data.StoryCategoryTitle,
                TotalViews = totalViews,
                UniqueViewers = uniqueViewers,
                LikesCount = data.LikesCount,
                SharesCount = data.SharesCount,
                RatingsCount = data.RatingsCount,
                AverageRating = avgRating,
                EngagementScore = engagementScore,
                RepeatViewerRate = repeatViewerRate
            };
        }).ToList();

        // Sort based on SortBy parameter
        var sortedStories = request.SortBy switch
        {
            SortBy.Views => performanceList.OrderByDescending(p => p.TotalViews),
            SortBy.Likes => performanceList.OrderByDescending(p => p.LikesCount),
            SortBy.Shares => performanceList.OrderByDescending(p => p.SharesCount),
            SortBy.Rating => performanceList.OrderByDescending(p => p.AverageRating ?? 0),
            SortBy.Engagement => performanceList.OrderByDescending(p => p.EngagementScore),
            _ => performanceList.OrderByDescending(p => p.TotalViews)
        };

        var response = new GetTopStoriesQueryResponse
        {
            Stories = sortedStories.Take(request.Top).ToList()
        };

        return timeZoneConverter.ConvertUtcToLocal(response);
    }

    private static decimal CalculateEngagementScore(int views, int likes, int shares, int ratings)
    {
        if (views == 0) return 0;

        var likeRate = (decimal)likes / views;
        var shareRate = (decimal)shares / views;
        var ratingRate = (decimal)ratings / views;

        // Weighted formula: 40% like rate, 40% share rate, 20% rating rate
        var score = (likeRate * 0.4m) + (shareRate * 0.4m) + (ratingRate * 0.2m);

        return Math.Round(score * 100, 2); // Convert to 0-100 scale
    }
}
