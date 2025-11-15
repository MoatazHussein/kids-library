using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTopStories;

public class GetTopStoriesQueryResponse
{
    public List<StoryPerformanceDto> Stories { get; set; } = [];
}
