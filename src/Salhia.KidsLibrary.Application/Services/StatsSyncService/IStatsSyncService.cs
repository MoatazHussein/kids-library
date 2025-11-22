namespace Salhia.KidsLibrary.Application.Services.StatsSyncService;

public interface IStatsSyncService
{
    Task SyncStatsAsync(CancellationToken cancellationToken = default);
    Task SyncStoryStatsAsync(string storyId, CancellationToken cancellationToken = default);
}
