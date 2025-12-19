using Microsoft.Extensions.Logging;
using Quartz;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.Infrastructure.Jobs;

/// <summary>
/// Quartz job for synchronizing story statistics.
/// </summary>
[DisallowConcurrentExecution] // Prevent multiple instances running simultaneously
public class StatsSyncJob(
    IStatsSyncService statsSyncService,
    ILogger<StatsSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var jobId = context.FireInstanceId;
        var scheduledTime = context.ScheduledFireTimeUtc?.LocalDateTime;
        
        logger.LogInformation(
            "Starting scheduled stats synchronization. JobId={JobId}, ScheduledTime={ScheduledTime}",
            jobId, scheduledTime);

        try
        {
            var result = await statsSyncService.SyncAllStatsAsync(context.CancellationToken);

            logger.LogInformation(
                "Scheduled stats sync completed successfully. " +
                "TotalSynced={TotalSynced}, Created={Created}, Updated={Updated}",
                result.TotalSynced, result.Created, result.Updated);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during scheduled stats synchronization");
            
            // Rethrow to let Quartz handle retry logic
            throw new JobExecutionException(ex, refireImmediately: false);
        }
    }
}
