using MediatR;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Queries.GetAIStoryById;

public class GetAIStoryByIdQuery : IRequest<GetAIStoryByIdQueryResponse>
{
    public string Id { get; set; } = string.Empty;
    
    // Items (Slides) pagination
    public int SlidesPageNumber { get; set; } = 1;
    public int SlidesPageSize { get; set; } = 10;
    public string? SlidesOrderBy { get; set; }
    public bool SlidesDescending { get; set; } = false;
}
