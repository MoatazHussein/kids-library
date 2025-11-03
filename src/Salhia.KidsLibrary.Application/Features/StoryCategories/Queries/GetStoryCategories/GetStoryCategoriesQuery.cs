using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;

public class GetStoryCategoriesQuery : IRequest<PagedResult<GetStoryCategoriesQueryResponse>>
{
    public string? SearchPhrase { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
