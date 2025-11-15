using MediatR;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetStoryEngagement;

public record GetStoryEngagementQuery(
    string StoryId,
    DashboardPeriod Period = DashboardPeriod.Last30Days
) : IRequest<GetStoryEngagementQueryResponse>;
