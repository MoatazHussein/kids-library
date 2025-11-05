using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.StoryComments.Queries.GetStoryComments;

public class GetStoryCommentsQuery : IRequest<PagedResult<GetStoryCommentsQueryResponse>>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public string? SearchPhrase { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
