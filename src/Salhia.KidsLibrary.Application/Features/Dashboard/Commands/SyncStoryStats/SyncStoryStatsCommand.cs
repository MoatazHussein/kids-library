using MediatR;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncStoryStats;

public class SyncStoryStatsCommand : IRequest<SyncStatsResult>
{
    public string StoryId { get; set; } = default!;
}
