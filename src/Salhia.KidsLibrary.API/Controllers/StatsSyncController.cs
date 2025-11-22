using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salhia.KidsLibrary.Application.Services.StatsSyncService;
using Salhia.KidsLibrary.Domain.Constants;

namespace Salhia.KidsLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class StatsSyncController(
    IStatsSyncService statsSyncService,
    ILogger<StatsSyncController> logger
    ) : ControllerBase
{
    /// <summary>
    /// Manually trigger a full stats synchronization (Admin only)
    /// </summary>
    [HttpPost("sync-all")]
    public async Task<IActionResult> SyncAllStats(CancellationToken cancellationToken)
    {
        logger.LogInformation("Manual stats sync triggered by admin");

        await statsSyncService.SyncStatsAsync(cancellationToken);

        return Ok(new { message = "Stats synchronization completed successfully" });
    }

    /// <summary>
    /// Manually sync stats for a specific story (Admin only)
    /// </summary>
    [HttpPost("sync/{storyId}")]
    public async Task<IActionResult> SyncStoryStats(string storyId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Manual stats sync for story {StoryId} triggered by admin", storyId);

        await statsSyncService.SyncStoryStatsAsync(storyId, cancellationToken);

        return Ok(new { message = $"Stats for story {storyId} synchronized successfully" });
    }
}
