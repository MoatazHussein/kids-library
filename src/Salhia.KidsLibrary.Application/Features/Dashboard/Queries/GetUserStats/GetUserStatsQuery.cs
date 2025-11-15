using MediatR;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetUserStats;

public record GetUserStatsQuery(
    string UserId,
    DashboardPeriod Period = DashboardPeriod.Last30Days,
    bool IncludeTopCategories = true,
    bool IncludeDailyActivity = true,
    int TopCategoriesLimit = 5
) : IRequest<GetUserStatsQueryResponse>;
