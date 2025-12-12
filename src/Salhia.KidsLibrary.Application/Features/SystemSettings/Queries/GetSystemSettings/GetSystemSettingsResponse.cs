
namespace Salhia.KidsLibrary.Application.Features.SystemSettings.Queries.GetSystemSettings;

public record GetSystemSettingsResponse
{
    public int AIStoryLimitCount { get; set; }
    public int AIStoryLimitDays { get; set; }
    public DateTime CreatedAt { get; set; }
}
