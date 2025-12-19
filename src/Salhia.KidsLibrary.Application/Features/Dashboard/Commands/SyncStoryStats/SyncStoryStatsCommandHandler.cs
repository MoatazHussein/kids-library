using MediatR;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncStoryStats;

public class SyncStoryStatsCommandHandler(
    IStatsSyncService statsSyncService
) : IRequestHandler<SyncStoryStatsCommand, SyncStatsResult>
{
    public async Task<SyncStatsResult> Handle(SyncStoryStatsCommand request, CancellationToken cancellationToken)
    {
        return await statsSyncService.SyncStoryStatsAsync(request.StoryId, cancellationToken);
    }
}
