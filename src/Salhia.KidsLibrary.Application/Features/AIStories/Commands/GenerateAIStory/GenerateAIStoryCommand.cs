using MediatR;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.GenerateAIStory;

public class GenerateAIStoryCommand : IRequest<GenerateAIStoryCommandResponse>
{
    public string CustomStoryId { get; set; } = default!;
    public string StoryName { get; set; } = default!;
    public string HeroName { get; set; } = default!;
    public string HeroImageUrl { get; set; } = default!;
}
