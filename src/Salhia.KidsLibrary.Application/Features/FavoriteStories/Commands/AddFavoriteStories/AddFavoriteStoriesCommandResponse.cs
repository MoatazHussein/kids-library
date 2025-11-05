namespace Salhia.KidsLibrary.Application.Features.FavoriteStories.Commands.AddFavoriteStories;

public class AddFavoriteStoriesCommandResponse
{
    public int TotalRequested { get; set; }
    public int NewlyAdded { get; set; }
    public int AlreadyFavorited { get; set; }
    public List<string> AddedStoryIds { get; set; } = new();
}
