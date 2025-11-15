using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetStoryEngagement;

public class GetStoryEngagementQueryHandler(
    IDashboardRepository dashboardRepository,
    ITimeZoneConverter timeZoneConverter,
    ILogger<GetStoryEngagementQueryHandler> logger
) : IRequestHandler<GetStoryEngagementQuery, GetStoryEngagementQueryResponse>
{
    public async Task<GetStoryEngagementQueryResponse> Handle(
        GetStoryEngagementQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting engagement metrics for story {StoryId} with period {Period}",
            request.StoryId, request.Period);

        // Calculate period date
        var periodFrom = GetPeriodStartDate(request.Period);

        // Get engagement data from repository (optimized query with daily breakdown)
        var engagementData = await dashboardRepository.GetStoryEngagementAsync(
            request.StoryId,
            periodFrom,
            cancellationToken);

        // Map to DTO with calculated rates
        var periodViewsCount = engagementData.PeriodViews;
        var likeRate = periodViewsCount > 0
            ? Math.Round((decimal)engagementData.PeriodLikes / periodViewsCount * 100, 2)
            : 0;
        var shareRate = periodViewsCount > 0
            ? Math.Round((decimal)engagementData.PeriodShares / periodViewsCount * 100, 2)
            : 0;
        var commentRate = periodViewsCount > 0
            ? Math.Round((decimal)engagementData.PeriodComments / periodViewsCount * 100, 2)
            : 0;

        var uniqueViewers = engagementData.UniqueViewers;
        var returnRate = uniqueViewers > 0
            ? Math.Round((decimal)engagementData.ReturningViewers / uniqueViewers * 100, 2)
            : 0;
        var avgViewsPerVisitor = uniqueViewers > 0
            ? Math.Round((decimal)periodViewsCount / uniqueViewers, 2)
            : 0;

        var avgRating = engagementData.TotalRatings > 0
            ? Math.Round((decimal)engagementData.RatingsSum / engagementData.TotalRatings, 2)
            : 0;

        var engagement = new StoryEngagementDto
        {
            Id = engagementData.Id,
            Title = engagementData.Title,
            CoverImageUrl = engagementData.CoverImageUrl,
            TotalViews = engagementData.TotalViews,
            TotalLikes = engagementData.TotalLikes,
            TotalShares = engagementData.TotalShares,
            TotalComments = engagementData.TotalComments,
            TotalRatings = engagementData.TotalRatings,
            AverageRating = avgRating,
            PeriodViews = periodViewsCount,
            PeriodLikes = engagementData.PeriodLikes,
            PeriodShares = engagementData.PeriodShares,
            PeriodComments = engagementData.PeriodComments,
            PeriodRatings = engagementData.PeriodRatings,
            LikeRate = likeRate,
            ShareRate = shareRate,
            CommentRate = commentRate,
            UniqueViewers = uniqueViewers,
            ReturningViewers = engagementData.ReturningViewers,
            ReturnRate = returnRate,
            AverageViewsPerVisitor = avgViewsPerVisitor,
            DailyBreakdown = engagementData.DailyBreakdown.Select(d => new DailyEngagementDto
            {
                Date = d.Date,
                Views = d.Views,
                Likes = d.Likes,
                Shares = d.Shares,
                Comments = d.Comments
            }).ToList()
        };

        var response = new GetStoryEngagementQueryResponse
        {
            Engagement = engagement
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
}
