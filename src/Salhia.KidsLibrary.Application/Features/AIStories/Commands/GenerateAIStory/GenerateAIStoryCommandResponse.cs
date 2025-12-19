namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;

public class GenerateAIStoryCommandResponse
{
    public string AIStoryId { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Status { get; set; } = default!;
}
