using MediatR;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStoryById;

public class GetCustomStoryByIdQuery : IRequest<GetCustomStoryByIdQueryResponse>
{
    public string Id { get; set; } = string.Empty;
    
    // Items pagination
    public int ItemsPageNumber { get; set; } = 1;
    public int ItemsPageSize { get; set; } = 10;
    public string? ItemsOrderBy { get; set; }
    public bool ItemsDescending { get; set; } = false;
}
