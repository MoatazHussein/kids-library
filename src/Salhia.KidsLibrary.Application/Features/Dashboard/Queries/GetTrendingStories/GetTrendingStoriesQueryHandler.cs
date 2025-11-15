using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTrendingStories;

public class GetTrendingStoriesQueryHandler(
    IDashboardRepository dashboardRepository,
    ITimeZoneConverter timeZoneConverter,
    ILogger<GetTrendingStoriesQueryHandler> logger
) : IRequestHandler<GetTrendingStoriesQuery, GetTrendingStoriesQueryResponse>
{
    public async Task<GetTrendingStoriesQueryResponse> Handle(
        GetTrendingStoriesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting top {Top} trending stories for period {Period}",
            request.Top, request.Period);

        // Calculate period dates
        var (recentFrom, comparisonFrom) = GetPeriodDates(request.Period);

        // Get trending data from repository (optimized single query)
        var trendingData = await dashboardRepository.GetTrendingStoriesAsync(
            recentFrom,
            comparisonFrom,
            request.Top,
            request.CategoryId,
            cancellationToken);

        // Map to DTOs with calculated trending score
        var trendingList = trendingData.Select(data =>
        {
            // Calculate growth rate
            var growthRate = data.ComparisonViews > 0
                ? Math.Round(((decimal)(data.RecentViews - data.ComparisonViews) / data.ComparisonViews) * 100, 2)
                : (data.RecentViews > 0 ? 100m : 0m); // 100% growth if new activity

            // Calculate trending score (weighted: views 50%, likes 30%, shares 20%)
            var trendingScore = CalculateTrendingScore(data.RecentViews, data.RecentLikes, data.RecentShares);

            return new TrendingStoryDto
            {
                Id = data.Id,
                Title = data.Title,
                CoverImageUrl = data.CoverImageUrl,
                StoryCategoryId = data.StoryCategoryId,
                StoryCategoryTitle = data.StoryCategoryTitle,
                RecentViews = data.RecentViews,
                RecentLikes = data.RecentLikes,
                RecentShares = data.RecentShares,
                TrendingScore = trendingScore,
                ViewsGrowthRate = growthRate,
                NewViewers = data.NewViewers
            };
        })
        .OrderByDescending(t => t.TrendingScore)
        .Take(request.Top)
        .ToList();

        var response = new GetTrendingStoriesQueryResponse
        {
            Stories = trendingList
        };

        return timeZoneConverter.ConvertUtcToLocal(response);
    }

    private static (DateTime RecentFrom, DateTime ComparisonFrom) GetPeriodDates(DashboardPeriod period)
    {
        var now = DateTime.UtcNow;
        var days = period switch
        {
            DashboardPeriod.Last24Hours => 1,
            DashboardPeriod.Last7Days => 7,
            DashboardPeriod.Last30Days => 30,
            DashboardPeriod.Last90Days => 90,
            _ => 7
        };

        var recentFrom = now.AddDays(-days);
        var comparisonFrom = now.AddDays(-days * 2); // Compare to previous period

        return (recentFrom, comparisonFrom);
    }

    private static int CalculateTrendingScore(int views, int likes, int shares)
    {
        // Weighted scoring: views (50%), likes (30%), shares (20%)
        // Normalize to 0-100 scale
        var viewsScore = Math.Min(views * 0.5, 50);
        var likesScore = Math.Min(likes * 3, 30);
        var sharesScore = Math.Min(shares * 10, 20);

        return (int)Math.Round(viewsScore + likesScore + sharesScore);
    }
}
