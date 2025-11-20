using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Services.StoryNotificationService;

public interface IStoryNotificationService
{
    Task NotifyAdminsOfNewStoryAsync(MasterStory story, string authorId, string categoryTitle, int maxAdmins = 5, CancellationToken ct = default);
    Task NotifyAuthorOfApprovalAsync(MasterStory story, CancellationToken ct = default);
}
