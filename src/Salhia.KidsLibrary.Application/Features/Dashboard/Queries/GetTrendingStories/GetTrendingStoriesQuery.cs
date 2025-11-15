using MediatR;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTrendingStories;

public record GetTrendingStoriesQuery(
    int Top = 10,
    DashboardPeriod Period = DashboardPeriod.Last7Days,
    string? CategoryId = null
) : IRequest<GetTrendingStoriesQueryResponse>;
