using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Services.AIStoryImageProcessing;

namespace Salhia.KidsLibrary.Application.Features.AIStories.Commands.RetryAIStorySlide;

public class RetryAIStorySlideCommandHandler(
    IAIStoryImageProcessingService processingService,
    ILogger<RetryAIStorySlideCommandHandler> logger) : IRequestHandler<RetryAIStorySlideCommand, RetryAIStorySlideCommandResponse>
{
    public async Task<RetryAIStorySlideCommandResponse> Handle(RetryAIStorySlideCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrying slide {SlideId}", request.SlideId);
        
        await processingService.RetrySlideAsync(request.SlideId, cancellationToken);
        
        return new RetryAIStorySlideCommandResponse
        {
            SlideId = request.SlideId,
            Message = "Slide regeneration started successfully",
            Status = "Image is being regenerated. Poll the story endpoint to check progress."
        };
    }
}
