using MediatR;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStoryById;

public class GetMasterStoryByIdQuery : IRequest<GetMasterStoryByIdQueryResponse>
{
    public string Id { get; set; } = string.Empty;
    
    // Comments pagination
    public int CommentsPageNumber { get; set; } = 1;
    public int CommentsPageSize { get; set; } = 10;
}
