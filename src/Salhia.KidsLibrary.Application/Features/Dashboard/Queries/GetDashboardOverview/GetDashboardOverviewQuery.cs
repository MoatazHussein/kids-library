using MediatR;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetDashboardOverview;

public class GetDashboardOverviewQuery : IRequest<GetDashboardOverviewQueryResponse>
{
    public DashboardPeriod Period { get; set; } = DashboardPeriod.Last30Days;
}
