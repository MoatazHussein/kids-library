using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTopStories;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTrendingStories;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetStoryEngagement;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetUserStats;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetDashboardOverview;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get top performing stories based on various metrics
    /// </summary>
    /// <param name="query">Query parameters for filtering and sorting</param>
    /// <returns>List of top performing stories with engagement metrics</returns>
    [HttpPost("stories/top")]
    public async Task<ActionResult<GetTopStoriesQueryResponse>> GetTopStories(
        [FromQuery] GetTopStoriesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get trending stories with recent activity and growth metrics
    /// </summary>
    /// <param name="query">Query parameters for period and filtering</param>
    /// <returns>List of trending stories with growth indicators</returns>
    [HttpPost("stories/trending")]
    public async Task<ActionResult<GetTrendingStoriesQueryResponse>> GetTrendingStories(
        [FromQuery] GetTrendingStoriesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get detailed engagement metrics for a specific story
    /// </summary>
    /// <param name="query">Story ID and period parameters</param>
    /// <returns>Comprehensive engagement analysis including daily breakdown</returns>
    [HttpPost("stories/{storyId}/engagement")]
    public async Task<ActionResult<GetStoryEngagementQueryResponse>> GetStoryEngagement(
        [FromRoute] string storyId,
        [FromQuery] DashboardPeriod period = DashboardPeriod.Last30Days)
    {
        var query = new GetStoryEngagementQuery(storyId, period);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get comprehensive user statistics and activity metrics
    /// </summary>
    /// <param name="userId">User ID to get stats for</param>
    /// <param name="period">Time period for analysis</param>
    /// <param name="includeTopCategories">Include top viewed categories</param>
    /// <param name="includeDailyActivity">Include daily activity breakdown</param>
    /// <param name="topCategoriesLimit">Number of top categories to return</param>
    /// <returns>User statistics with engagement metrics and behavior insights</returns>
    [HttpPost("users/{userId}/stats")]
    public async Task<ActionResult<GetUserStatsQueryResponse>> GetUserStats(
        [FromRoute] string userId,
        [FromQuery] DashboardPeriod period = DashboardPeriod.Last30Days,
        [FromQuery] bool includeTopCategories = true,
        [FromQuery] bool includeDailyActivity = true,
        [FromQuery] int topCategoriesLimit = 5)
    {
        var query = new GetUserStatsQuery(userId, period, includeTopCategories, includeDailyActivity, topCategoriesLimit);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get dashboard overview with comprehensive summary metrics
    /// </summary>
    /// <param name="period">Time period for analysis (default: Last30Days)</param>
    /// <returns>Dashboard overview with story counts, user counts, engagement totals and rates</returns>
    [AllowAnonymous]
    [HttpPost("overview")]
    public async Task<ActionResult<GetDashboardOverviewQueryResponse>> GetDashboardOverview(
        [FromQuery] DashboardPeriod period = DashboardPeriod.Last30Days)
    {
        var query = new GetDashboardOverviewQuery { Period = period };
        var result = await mediator.Send(query);
        return Ok(result);
    }
}
