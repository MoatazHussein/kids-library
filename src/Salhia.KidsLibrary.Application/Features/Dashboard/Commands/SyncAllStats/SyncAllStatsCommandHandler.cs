using MediatR;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncAllStats;

public class SyncAllStatsCommandHandler(
    IStatsSyncService statsSyncService
) : IRequestHandler<SyncAllStatsCommand, SyncStatsResult>
{
    public async Task<SyncStatsResult> Handle(SyncAllStatsCommand request, CancellationToken cancellationToken)
    {
        return await statsSyncService.SyncAllStatsAsync(cancellationToken);
    }
}
