using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Services.StatsSync;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Infrastructure.Services.StatsSync;

public class StatsSyncService(
    IRepository<MasterStory> storyRepository,
    IRepository<MasterStoryStats> statsRepository,
    IUnitOfWork unitOfWork,
    ILogger<StatsSyncService> logger
) : IStatsSyncService
{
    public async Task<SyncStatsResult> SyncAllStatsAsync(CancellationToken cancellationToken = default)
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
            statsDict.TryGetValue(story.Id, out var stats);

            // Calculate actual counts from real data
            var actualRatingsCount = story.Ratings.Count;
            var actualRatingsSum = story.Ratings.Sum(r => r.Rating);
            var actualLikesCount = story.Likes.Count;
            var actualSharesCount = story.Shares.Count;
            var actualTotalViews = story.ViewSessions.Sum(vs => vs.ViewCount);

            if (stats == null)
            {
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
                logger.LogDebug("Created stats for story {StoryId}", story.Id);
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
                    logger.LogDebug("Updated stats for story {StoryId}", story.Id);
                }
            }

            syncedCount++;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Stats sync completed: {TotalSynced} stories processed, {Created} created, {Updated} updated",
            syncedCount, createdCount, updatedCount);

        return new SyncStatsResult
        {
            TotalSynced = syncedCount,
            Created = createdCount,
            Updated = updatedCount,
            Message = "Stats synchronization completed successfully"
        };
    }

    public async Task<SyncStatsResult> SyncStoryStatsAsync(string storyId, CancellationToken cancellationToken = default)
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
            throw new AppException($"Story with ID {storyId} not found");
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

        var created = 0;
        var updated = 0;

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
            created = 1;
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
            updated = 1;
            logger.LogInformation("Updated stats for story {StoryId}", storyId);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SyncStatsResult
        {
            TotalSynced = 1,
            Created = created,
            Updated = updated,
            Message = $"Stats for story {storyId} synchronized successfully"
        };
    }
}
