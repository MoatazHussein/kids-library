using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Services.StoryNotificationService;

public class StoryNotificationService(
    IUserService userService,
    IMailService mailService,
    ILogger<StoryNotificationService> logger
    ) : IStoryNotificationService
{
    public async Task NotifyAdminsOfNewStoryAsync(MasterStory story, string authorId, string categoryTitle, int maxAdmins = 5, CancellationToken ct = default)
    {
        var allAdmins = await userService.GetAllAdminsAsync(ct);

        if (allAdmins.Count == 0)
        {
            logger.LogWarning("No admin users found to notify about new story {StoryId}", story.Id);
            return;
        }

        // Select top oldest admins (by CreatedAt ascending)
        var admins = allAdmins
            .OrderBy(a => a.CreatedAt)
            .Take(maxAdmins)
            .ToList();

        logger.LogInformation("Notifying {Count} admin(s) out of {Total} total admins for story {StoryId}", admins.Count, allAdmins.Count, story.Id);

        var author = await userService.FindByIdAsync(authorId, ct);
        var authorName = author != null ? $"{author.FirstName} {author.LastName}" : "A user";

        var emailBody = $@"
            <h2>New Story Pending Approval</h2>
            <p>Dear Admin,</p>
            <p><strong>{authorName}</strong> has submitted a new story that requires your approval.</p>
            <p><strong>Story Title:</strong> {story.Title}</p>
            <p><strong>Category:</strong> {categoryTitle}</p>
            <p>Please review and approve the story at your earliest convenience.</p>
            <br/>
            <p>Best regards,<br/>Salhia Kids Library System</p>";

        foreach (var admin in admins)
        {
            if (!string.IsNullOrEmpty(admin.Email))
            {
                await mailService.SendEmailAsync(admin.Email, "New Story Awaiting Approval", emailBody, null);
                logger.LogInformation("Sent new story notification to admin {AdminEmail} for story {StoryId}", admin.Email, story.Id);
            }
        }
    }

    public async Task NotifyAuthorOfApprovalAsync(MasterStory story, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(story.CreatedBy))
        {
            logger.LogWarning("Story {StoryId} has no author to notify", story.Id);
            return;
        }

        var author = await userService.FindByIdAsync(story.CreatedBy, ct);
        if (author?.Email == null)
        {
            logger.LogWarning("Author {AuthorId} not found or has no email for story approval notification", story.CreatedBy);
            return;
        }

        var emailBody = $@"
            <h2>Story Approved!</h2>
            <p>Dear {author.FirstName} {author.LastName},</p>
            <p>Great news! Your story <strong>'{story.Title}'</strong> has been approved and is now visible to all users.</p>
            <p>Thank you for your contribution!</p>
            <br/>
            <p>Best regards,<br/>Salhia Kids Library Team</p>";

        await mailService.SendEmailAsync(author.Email, "Your Story Has Been Approved", emailBody, null);

    }
}
