namespace Salhia.KidsLibrary.Application.Services.StatsSync;

public interface IStatsSyncService
{
    Task<SyncStatsResult> SyncAllStatsAsync(CancellationToken cancellationToken = default);
    Task<SyncStatsResult> SyncStoryStatsAsync(string storyId, CancellationToken cancellationToken = default);
}
