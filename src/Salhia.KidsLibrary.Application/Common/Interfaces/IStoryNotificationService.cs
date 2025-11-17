using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Common.Interfaces;

public interface IStoryNotificationService
{
    Task NotifyAdminsOfNewStoryAsync(MasterStory story, string authorId, string categoryTitle, CancellationToken ct = default);
    Task NotifyAuthorOfApprovalAsync(MasterStory story, CancellationToken ct = default);
}
