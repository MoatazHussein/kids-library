namespace Salhia.KidsLibrary.Application.Features.StoryCategories.Queries.GetStoryCategories;

public class GetStoryCategoriesQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int MasterStoriesCount { get; set; }
}
