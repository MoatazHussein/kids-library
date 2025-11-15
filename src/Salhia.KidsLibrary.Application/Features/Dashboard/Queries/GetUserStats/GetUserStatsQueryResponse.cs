using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetUserStats;

public class GetUserStatsQueryResponse
{
    public UserStatsDto Stats { get; set; } = null!;
}
