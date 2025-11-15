using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Services.StoryViewService;

public class StoryViewService(
    IRepository<StoryViewSession> viewSessionRepository,
    IRepository<MasterStoryStats> storyStatsRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
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
            // 2) Increment stats
            var stats = await storyStatsRepository
                .FirstOrDefaultAsync(
                    s => s.MasterStoryId == storyId,
                    cancellationToken: cancellationToken);

            if (stats is null)
            {
                stats = new MasterStoryStats
                {
                    MasterStoryId = storyId,
                    TotalViews = 1,
                    CreatedAt = nowUtc,
                    UpdatedAt = nowUtc
                };

                await storyStatsRepository.AddAsync(stats, cancellationToken);
            }
            else
            {
                stats.TotalViews += 1;
                stats.UpdatedAt = nowUtc;
                await storyStatsRepository.UpdateAsync(stats);
            }
        }

        // 3) Single SaveChanges – Repository and UnitOfWork share the same DbContext
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
