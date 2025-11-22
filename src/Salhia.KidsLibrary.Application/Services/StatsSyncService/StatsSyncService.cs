using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Services.StatsSyncService;

public class StatsSyncService(
    IRepository<MasterStory> storyRepository,
    IRepository<MasterStoryStats> statsRepository,
    IUnitOfWork unitOfWork,
    ILogger<StatsSyncService> logger
    ) : IStatsSyncService
{
    public async Task SyncStatsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting full stats synchronization");

        // Get all approved stories
        var stories = await storyRepository.GetAllAsync(
            s => s.ApprovalStatus == ApprovalStatus.Approved,
            cancellationToken,
            [
                s => s.Ratings,
                s => s.Likes,
                s => s.Shares,
                s => s.ViewSessions
            ]);

        // Load all existing stats in one query to avoid N+1
        var storyIds = stories.Select(s => s.Id).ToList();
        var allStats = await statsRepository.GetAllAsync(
            s => storyIds.Contains(s.MasterStoryId),
            cancellationToken);

        // Create a dictionary for O(1) lookup
        var statsDict = allStats.ToDictionary(s => s.MasterStoryId);

        var syncedCount = 0;
        var createdCount = 0;
        var updatedCount = 0;

        foreach (var story in stories)
        {
            // O(1) lookup instead of database query
            statsDict.TryGetValue(story.Id, out var stats);

            // Calculate actual counts from real data
            var actualRatingsCount = story.Ratings.Count;
            var actualRatingsSum = story.Ratings.Sum(r => r.Rating);
            var actualLikesCount = story.Likes.Count;
            var actualSharesCount = story.Shares.Count;
            var actualTotalViews = story.ViewSessions.Sum(vs => vs.ViewCount);

            if (stats == null)
            {
                // Create new stats record
                stats = new MasterStoryStats
                {
                    MasterStoryId = story.Id,
                    RatingsCount = actualRatingsCount,
                    RatingsSum = actualRatingsSum,
                    LikesCount = actualLikesCount,
                    SharesCount = actualSharesCount,
                    TotalViews = actualTotalViews,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                await statsRepository.AddAsync(stats, cancellationToken);
                createdCount++;
                logger.LogInformation("Created stats for story {StoryId}: R={Ratings}, L={Likes}, S={Shares}, V={Views}",
                    story.Id, actualRatingsCount, actualLikesCount, actualSharesCount, actualTotalViews);
            }
            else
            {
                // Check if update is needed
                if (stats.RatingsCount != actualRatingsCount ||
                    stats.RatingsSum != actualRatingsSum ||
                    stats.LikesCount != actualLikesCount ||
                    stats.SharesCount != actualSharesCount ||
                    stats.TotalViews != actualTotalViews)
                {
                    stats.RatingsCount = actualRatingsCount;
                    stats.RatingsSum = actualRatingsSum;
                    stats.LikesCount = actualLikesCount;
                    stats.SharesCount = actualSharesCount;
                    stats.TotalViews = actualTotalViews;
                    stats.UpdatedAt = DateTime.UtcNow;
                    stats.UpdatedBy = "system";

                    await statsRepository.UpdateAsync(stats);
                    updatedCount++;
                    logger.LogInformation("Updated stats for story {StoryId}: R={Ratings}, L={Likes}, S={Shares}, V={Views}",
                        story.Id, actualRatingsCount, actualLikesCount, actualSharesCount, actualTotalViews);
                }
            }

            syncedCount++;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Stats sync completed: {TotalSynced} stories processed, {Created} created, {Updated} updated",
            syncedCount, createdCount, updatedCount);
    }

    public async Task SyncStoryStatsAsync(string storyId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Syncing stats for story {StoryId}", storyId);

        var story = await storyRepository.GetByIdAsync(
            storyId,
            cancellationToken,
            [
                s => s.Ratings,
                s => s.Likes,
                s => s.Shares,
                s => s.ViewSessions
            ]);

        if (story == null)
        {
            logger.LogWarning("Story {StoryId} not found for stats sync", storyId);
            return;
        }

        var stats = await statsRepository.FirstOrDefaultAsync(
            s => s.MasterStoryId == storyId,
            cancellationToken);

        // Calculate actual counts
        var actualRatingsCount = story.Ratings.Count;
        var actualRatingsSum = story.Ratings.Sum(r => r.Rating);
        var actualLikesCount = story.Likes.Count;
        var actualSharesCount = story.Shares.Count;
        var actualTotalViews = story.ViewSessions.Sum(vs => vs.ViewCount);

        if (stats == null)
        {
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                RatingsCount = actualRatingsCount,
                RatingsSum = actualRatingsSum,
                LikesCount = actualLikesCount,
                SharesCount = actualSharesCount,
                TotalViews = actualTotalViews,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            await statsRepository.AddAsync(stats, cancellationToken);
            logger.LogInformation("Created stats for story {StoryId}", storyId);
        }
        else
        {
            stats.RatingsCount = actualRatingsCount;
            stats.RatingsSum = actualRatingsSum;
            stats.LikesCount = actualLikesCount;
            stats.SharesCount = actualSharesCount;
            stats.TotalViews = actualTotalViews;
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = "system";

            await statsRepository.UpdateAsync(stats);
            logger.LogInformation("Updated stats for story {StoryId}", storyId);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
