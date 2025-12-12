namespace Salhia.KidsLibrary.Application.Common.Dtos.AIStories;

public class AIStoryDto
{
    public string Id { get; set; } = default!;
    public string StoryName { get; set; } = default!;
    public string HeroName { get; set; } = default!;
    public string HeroImageUrl { get; set; } = default!;
    public int SlidesCount { get; set; }
    public string CustomStoryId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public List<AIStorySlideDto> Slides { get; set; } = [];
}

public class AIStorySlideDto
{
    public string Id { get; set; } = default!;
    public int Index { get; set; }
    public string? Title { get; set; }
    public string Description { get; set; } = default!;
    public string ImagePrompt { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
