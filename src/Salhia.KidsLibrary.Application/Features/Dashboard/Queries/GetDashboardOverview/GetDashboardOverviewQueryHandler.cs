using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetDashboardOverview;

public class GetDashboardOverviewQueryHandler(
    IDashboardRepository dashboardRepository,
    ILogger<GetDashboardOverviewQueryHandler> logger
) : IRequestHandler<GetDashboardOverviewQuery, GetDashboardOverviewQueryResponse>
{
    public async Task<GetDashboardOverviewQueryResponse> Handle(
        GetDashboardOverviewQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dashboard overview for period {Period}", request.Period);

        var overviewData = await dashboardRepository.GetDashboardOverviewAsync(request.Period, cancellationToken);

        // Calculate engagement rates
        var viewToLikeRate = overviewData.TotalViews > 0 
            ? (decimal)overviewData.TotalLikes / overviewData.TotalViews * 100 
            : 0;
        
        var viewToShareRate = overviewData.TotalViews > 0 
            ? (decimal)overviewData.TotalShares / overviewData.TotalViews * 100 
            : 0;
        
        var viewToCommentRate = overviewData.TotalViews > 0 
            ? (decimal)overviewData.TotalComments / overviewData.TotalViews * 100 
            : 0;

        var avgEngagementScore = overviewData.TotalViews > 0
            ? (decimal)(overviewData.TotalLikes + overviewData.TotalShares + overviewData.TotalComments) / overviewData.TotalViews * 100
            : 0;

        var dto = new DashboardOverviewDto
        {
            StoryCounts = new StoryCountsDto
            {
                Total = overviewData.TotalStories,
                TotalInPeriod = overviewData.TotalStoriesInPeriod,
                ApprovedInPeriod = overviewData.ApprovedStoriesInPeriod,
                PendingInPeriod = overviewData.PendingStoriesInPeriod,
                RejectedInPeriod = overviewData.RejectedStoriesInPeriod
            },
            UserCounts = new UserCountsDto
            {
                Total = overviewData.TotalUsers,
                ActiveUsers = overviewData.ActiveUsers,
                InactiveUsers = overviewData.InactiveUsers,
                ActiveUsersInPeriod = overviewData.ActiveUsersInPeriod,
                NewInPeriod = overviewData.NewUsersInPeriod
            },
            EngagementMetrics = new EngagementMetricsDto
            {
                TotalViews = overviewData.TotalViews,
                TotalLikes = overviewData.TotalLikes,
                TotalShares = overviewData.TotalShares,
                TotalComments = overviewData.TotalComments,
                TotalRatings = overviewData.TotalRatings,
                AverageRating = overviewData.TotalAverageRating,
                ViewsInPeriod = overviewData.ViewsInPeriod,
                LikesInPeriod = overviewData.LikesInPeriod,
                SharesInPeriod = overviewData.SharesInPeriod,
                CommentsInPeriod = overviewData.CommentsInPeriod,
                RatingsInPeriod = overviewData.RatingsInPeriod
            },
            EngagementRates = new EngagementRatesDto
            {
                ViewToLikeRate = Math.Round(viewToLikeRate, 2),
                ViewToShareRate = Math.Round(viewToShareRate, 2),
                ViewToCommentRate = Math.Round(viewToCommentRate, 2),
                AverageEngagementScore = Math.Round(avgEngagementScore, 2)
            },
            MediaTypeStats = overviewData.MediaTypeStats,
        };

        return new GetDashboardOverviewQueryResponse
        {
            Data = dto
        };
    }
}
