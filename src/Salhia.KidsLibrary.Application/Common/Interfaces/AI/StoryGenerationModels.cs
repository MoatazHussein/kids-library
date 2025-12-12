namespace Salhia.KidsLibrary.Application.Common.Interfaces.AI;

public class StorySlideContent
{
    public string? Title { get; set; }
    public string Description { get; set; } = default!;
    public string ImagePrompt { get; set; } = default!;
}

public class StoryGenerationResult
{
    public List<StorySlideContent> Slides { get; set; } = [];
}
