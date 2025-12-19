using MediatR;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.RetryAIStorySlide;

public class RetryAIStorySlideCommand : IRequest<RetryAIStorySlideCommandResponse>
{
    public string SlideId { get; set; } = default!;
}
