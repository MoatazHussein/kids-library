using MediatR;
using Salhia.KidsLibrary.Application.Services.StatsSync;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Commands.SyncAllStats;

public class SyncAllStatsCommand : IRequest<SyncStatsResult>
{
}
