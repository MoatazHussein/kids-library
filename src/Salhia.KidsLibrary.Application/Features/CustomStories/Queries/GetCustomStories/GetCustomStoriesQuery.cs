using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.CustomStories.Queries.GetCustomStories;

public class GetCustomStoriesQuery : IRequest<PagedResult<GetCustomStoriesQueryResponse>>
{
    public string? SearchPhrase { get; set; }
    public string? CreatedBy { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
