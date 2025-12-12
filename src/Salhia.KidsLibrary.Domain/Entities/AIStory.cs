
namespace Salhia.KidsLibrary.Domain.Entities;

public class AIStory : BaseEntity
{
    public string StoryName { get; set; } = default!;
    public string HeroName { get; set; } = default!;
    public string HeroImageUrl { get; set; } = default!;
    public int SlidesCount { get; set; }

    public string CustomStoryId { get; set; } = default!;
    public CustomStory CustomStory { get; set; } = default!;

    public List<AIStorySlide> AIStorySlides { get; set; } = [];

    // User navigation properties
    public AppUser CreatedByUser { get; set; } = default!;
    public AppUser? UpdatedByUser { get; set; }
}
