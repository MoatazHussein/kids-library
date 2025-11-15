using MediatR;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTopStories;

public record GetTopStoriesQuery(
    int Top = 10,
    SortBy SortBy = SortBy.Views,
    string? CategoryId = null,
    DashboardPeriod Period = DashboardPeriod.AllTime
) : IRequest<GetTopStoriesQueryResponse>;
