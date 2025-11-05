using MediatR;
using Salhia.KidsLibrary.Application.Common.Models;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Queries.GetFavoriteStories;

public class GetFavoriteStoriesQuery : IRequest<PagedResult<GetFavoriteStoriesQueryResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchPhrase { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true; 
}
