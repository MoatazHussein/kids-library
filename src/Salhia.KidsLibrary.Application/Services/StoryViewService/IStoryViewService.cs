namespace Salhia.KidsLibrary.Application.Services.StoryViewService;

public interface IStoryViewService
{
    Task RegisterViewAsync(
       string storyId,
       string visitorKey,
       CancellationToken cancellationToken = default);
}
