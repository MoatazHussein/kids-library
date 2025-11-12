using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQuery : IRequest<PagedResult<GetMasterStoriesQueryResponse>>
{
    public string? StoryCategoryId { get; set; }
    public MediaType? MediaType { get; set; }
    public string? SearchPhrase { get; set; }
    public ApprovalStatus? ApprovalStatus { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
