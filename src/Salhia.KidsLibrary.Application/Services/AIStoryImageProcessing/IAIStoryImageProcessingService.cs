namespace Salhia.KidsLibrary.Application.Services.AIStoryImageProcessing;

public interface IAIStoryImageProcessingService
{
    Task ProcessStoryImmediatelyAsync(string storyId, CancellationToken cancellationToken = default);
    Task RetrySlideAsync(string slideId, CancellationToken cancellationToken = default);
}
