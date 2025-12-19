using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTopStories;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTrendingStories;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetStoryEngagement;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetUserStats;
using Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetDashboardOverview;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;
using Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncAllStats;
using Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncStoryStats;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "Admin")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpPost("stories/top")]
    public async Task<ActionResult<GetTopStoriesQueryResponse>> GetTopStories(
        [FromQuery] GetTopStoriesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("stories/trending")]
    public async Task<ActionResult<GetTrendingStoriesQueryResponse>> GetTrendingStories(
        [FromQuery] GetTrendingStoriesQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("stories/{storyId}/engagement")]
    public async Task<ActionResult<GetStoryEngagementQueryResponse>> GetStoryEngagement(
        [FromRoute] string storyId,
        [FromQuery] DashboardPeriod period = DashboardPeriod.Last30Days)
    {
        var query = new GetStoryEngagementQuery(storyId, period);
        var result = await mediator.Send(query);
        return Ok(result);
    }

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

    [HttpPost("overview")]
    public async Task<ActionResult<GetDashboardOverviewQueryResponse>> GetDashboardOverview(
        [FromQuery] DashboardPeriod period = DashboardPeriod.Last30Days)
    {
        var query = new GetDashboardOverviewQuery { Period = period };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("stats/sync-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SyncStatsResult>> SyncAllStats(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SyncAllStatsCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("stats/sync/{storyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SyncStatsResult>> SyncStoryStats([FromRoute] string storyId, CancellationToken cancellationToken)
    {
        var command = new SyncStoryStatsCommand { StoryId = storyId };
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
