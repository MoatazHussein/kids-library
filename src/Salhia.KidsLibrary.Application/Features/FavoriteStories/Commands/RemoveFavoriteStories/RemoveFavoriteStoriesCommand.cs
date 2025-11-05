using MediatR;

namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.RemoveFavoriteStories;

public class RemoveFavoriteStoriesCommand : IRequest<RemoveFavoriteStoriesCommandResponse>
{
    public List<string> MasterStoryIds { get; set; } = new();
}
