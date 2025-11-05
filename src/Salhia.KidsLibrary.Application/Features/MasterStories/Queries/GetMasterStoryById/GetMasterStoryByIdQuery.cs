using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQuery : IRequest<GetMasterStoryByIdQueryResponse>
{
    public string Id { get; set; } = string.Empty;
    
    // Media Items pagination
    public int MediaItemsPageNumber { get; set; } = 1;
    public int MediaItemsPageSize { get; set; } = 10;
    
    // Comments pagination
    public int CommentsPageNumber { get; set; } = 1;
    public int CommentsPageSize { get; set; } = 10;
}
