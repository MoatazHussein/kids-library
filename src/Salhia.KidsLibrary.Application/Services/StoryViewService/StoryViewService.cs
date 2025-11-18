using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Services.StoryViewService;

public class StoryViewService(
    IRepository<StoryViewSession> viewSessionRepository,
    IRepository<MasterStoryStats> storyStatsRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    ILogger<StoryViewService> logger
    ) : IStoryViewService
{
    public async Task RegisterViewAsync(
        string storyId,
        string visitorKey,
        CancellationToken cancellationToken = default)
    {
        const int dedupMinutes = 10;
        var minDelta = TimeSpan.FromMinutes(dedupMinutes);
        var nowUtc = DateTime.UtcNow;

        // Get userId from current user service if authenticated
        string? userId = currentUserService.IsAuthenticated ? currentUserService.UserId : null;

        // CRITICAL: Ensure stats exist BEFORE creating session to prevent orphaned records
        // This prevents race conditions where session is created but stats creation fails
        var stats = await storyStatsRepository
            .FirstOrDefaultAsync(
                s => s.MasterStoryId == storyId,
                cancellationToken: cancellationToken);

        if (stats is null)
        {
            logger.LogInformation("Creating initial stats record for story {StoryId}", storyId);
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                TotalViews = 0, // Will be incremented below if view is counted
                LikesCount = 0,
                SharesCount = 0,
                RatingsCount = 0,
                RatingsSum = 0,
                CreatedAt = nowUtc,
                UpdatedAt = nowUtc
            };

            await storyStatsRepository.AddAsync(stats, cancellationToken);
            // Save immediately to prevent race condition
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // 1) Try to find existing session for (Story, VisitorKey)
        var session = await viewSessionRepository
            .FirstOrDefaultAsync(
                s => s.MasterStoryId == storyId && s.VisitorKey == visitorKey,
                cancellationToken: cancellationToken);

        bool shouldCountView;

        if (session is null)
        {
            // First view for this (story, visitorKey)
            session = new StoryViewSession
            {
                MasterStoryId = storyId,
                VisitorKey = visitorKey,
                UserId = userId,
                LastViewAt = nowUtc,
                ViewCount = 1,
                CreatedAt = nowUtc,
            };

            await viewSessionRepository.AddAsync(session, cancellationToken);
            shouldCountView = true;
        }
        else
        {
            var delta = nowUtc - session.LastViewAt;
            if (delta >= minDelta)
            {
                session.LastViewAt = nowUtc;
                session.ViewCount++;
                session.UpdatedAt = nowUtc;
                await viewSessionRepository.UpdateAsync(session);

                shouldCountView = true;
            }
            else
            {
                shouldCountView = false;
            }
        }

        if (shouldCountView)
        {
            // 2) Increment stats (stats already exists, guaranteed by logic above)
            stats.TotalViews += 1;
            stats.UpdatedAt = nowUtc;
            await storyStatsRepository.UpdateAsync(stats);
            
            logger.LogDebug("Incremented views for story {StoryId} to {TotalViews}", storyId, stats.TotalViews);
        }

        // 3) Final SaveChanges for session and stats update
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
