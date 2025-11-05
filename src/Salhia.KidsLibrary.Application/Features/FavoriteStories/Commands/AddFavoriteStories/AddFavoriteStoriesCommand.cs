using MediatR;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.AddFavoriteStories;

public class AddFavoriteStoriesCommand : IRequest<AddFavoriteStoriesCommandResponse>
{
    public List<string> MasterStoryIds { get; set; } = new();
}
