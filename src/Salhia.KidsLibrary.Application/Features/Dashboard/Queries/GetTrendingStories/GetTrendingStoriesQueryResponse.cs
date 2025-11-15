using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Dtos;

namespace Salhia.KidsLibrary.Application.Features.Dashboard.Queries.GetTrendingStories;

public class GetTrendingStoriesQueryResponse
{
    public List<TrendingStoryDto> Stories { get; set; } = [];
}
