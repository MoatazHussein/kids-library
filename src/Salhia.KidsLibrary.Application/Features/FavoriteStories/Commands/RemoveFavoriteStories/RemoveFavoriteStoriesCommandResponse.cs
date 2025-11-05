namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.RemoveFavoriteStories;

public class RemoveFavoriteStoriesCommandResponse
{
    public int TotalRequested { get; set; }
    public int Removed { get; set; }
    public int NotFound { get; set; }
    public List<string> RemovedStoryIds { get; set; } = new();
}
