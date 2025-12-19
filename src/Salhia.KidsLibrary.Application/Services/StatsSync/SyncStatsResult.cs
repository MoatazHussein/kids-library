namespace Salhia.KidsLibrary.Application.Services.StatsSync;

public class SyncStatsResult
{
    public int TotalSynced { get; set; }
    public int Created { get; set; }
    public int Updated { get; set; }
    public string Message { get; set; } = default!;
}
