using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQuery : IRequest<PagedResult<GetMediaItemsQueryResponse>>
{
    public string MasterStoryId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchPhrase { get; set; }
}
