using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;

namespace Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;

public class MasterStoryStatsService(
    IRepository<MasterStoryStats> statsRepository,
    ILogger<MasterStoryStatsService> logger,
    ICurrentUserService currentUserService
    ) : IMasterStoryStatsService
{
    public async Task IncrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Incrementing rating stats for story {StoryId} with value {Rating}", storyId, ratingValue);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        if (stats is null)
        {
            // Create new stats record
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                RatingsCount = 1,
                RatingsSum = ratingValue,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
            };
            await statsRepository.AddAsync(stats, cancellationToken);
            logger.LogInformation("Created new stats record for story {StoryId}", storyId);
        }
        else
        {
            // Update existing stats
            stats.RatingsCount++;
            stats.RatingsSum += ratingValue;
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = currentUserService.UserId;

            await statsRepository.UpdateAsync(stats);
            logger.LogInformation("Updated stats: Count={Count}, Sum={Sum}, Avg={Avg:F2}",
                stats.RatingsCount, stats.RatingsSum, (decimal)stats.RatingsSum / stats.RatingsCount);
        }
    }

    public async Task UpdateRatingAsync(string storyId, int oldRating, int newRating, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating rating stats for story {StoryId} from {OldRating} to {NewRating}",
            storyId, oldRating, newRating);

        var stats = await statsRepository.FirstOrDefaultAsync(e=> e.MasterStoryId == storyId, cancellationToken);

        if (stats is not null)
        {
            var ratingDifference = newRating - oldRating;
            stats.RatingsSum += ratingDifference;
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = currentUserService.UserId;

            await statsRepository.UpdateAsync(stats);
            logger.LogInformation("Updated stats: Sum adjusted by {Difference}, new Sum={Sum}, Avg={Avg:F2}",
                ratingDifference, stats.RatingsSum, (decimal)stats.RatingsSum / stats.RatingsCount);
        }
        else
        {
            // Fallback: create stats if missing (shouldn't happen in normal flow)
            logger.LogWarning("Stats not found for story {StoryId}, creating new record", storyId);
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                RatingsCount = 1,
                RatingsSum = newRating,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
            };
            await statsRepository.AddAsync(stats, cancellationToken);
        }
    }

    public async Task DecrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Decrementing rating stats for story {StoryId} with value {Rating}", storyId, ratingValue);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        if (stats is not null)
        {
            stats.RatingsCount--;
            stats.RatingsSum -= ratingValue;
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = currentUserService.UserId;

            await statsRepository.UpdateAsync(stats);
            
            if (stats.RatingsCount > 0)
            {
                logger.LogInformation("Updated stats: RatingsCount={Count}, RatingsSum={Sum}, LikesCount={LikesCount}",
                    stats.RatingsCount, stats.RatingsSum, stats.LikesCount);
            }
            else
            {
                logger.LogInformation("Story {StoryId} has no ratings remaining (LikesCount={LikesCount})",
                    storyId, stats.LikesCount);
            }
        }
        else
        {
            logger.LogWarning("Stats not found for story {StoryId} when decrementing rating", storyId);
        }
    }

    public async Task<(int RatingsCount, decimal? AverageRating)> GetStoryRatingStatsAsync(
        string storyId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting rating stats for story {StoryId}", storyId);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        if (stats is null || stats.RatingsCount == 0)
        {
            return (0, null);
        }

        var averageRating = Math.Round((decimal)stats.RatingsSum / stats.RatingsCount, 2);
        return (stats.RatingsCount, averageRating);
    }

    public async Task<Dictionary<string, (int RatingsCount, decimal? AverageRating)>> GetMultipleStoryRatingStatsAsync(
        List<string> storyIds,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting rating stats for {Count} stories", storyIds.Count);

        var parameters = new QueryParameters<MasterStoryStats>
        {
            Filter = stat => storyIds.Contains(stat.MasterStoryId)
        };

        var (statsList, _) = await statsRepository.GetAllMatchingAsync(parameters, cancellationToken);

        var result = new Dictionary<string, (int RatingsCount, decimal? AverageRating)>();

        foreach (var stat in statsList)
        {
            var averageRating = stat.RatingsCount > 0
                ? Math.Round((decimal)stat.RatingsSum / stat.RatingsCount, 2)
                : (decimal?)null;

            result[stat.MasterStoryId] = (stat.RatingsCount, averageRating);
        }

        // Add entries for stories with no stats
        foreach (var storyId in storyIds)
        {
            if (!result.ContainsKey(storyId))
            {
                result[storyId] = (0, null);
            }
        }

        return result;
    }

    public async Task UpdateLikesCountAsync(string storyId, int increment, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating likes count for story {StoryId} by {Increment}", storyId, increment);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        if (stats is null)
        {
            // Create new stats record
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                RatingsCount = 0,
                RatingsSum = 0,
                LikesCount = Math.Max(0, increment), // Ensure non-negative
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserId
            };
            await statsRepository.AddAsync(stats, cancellationToken);
            logger.LogInformation("Created new stats record for story {StoryId} with LikesCount={LikesCount}", storyId, stats.LikesCount);
        }
        else
        {
            // Update existing stats
            stats.LikesCount = Math.Max(0, stats.LikesCount + increment); // Prevent negative count
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = currentUserService.UserId;

            await statsRepository.UpdateAsync(stats);
            logger.LogInformation("Updated likes count for story {StoryId}: LikesCount={LikesCount}", storyId, stats.LikesCount);
        }
    }

    public async Task<int> GetLikesCountAsync(string storyId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting likes count for story {StoryId}", storyId);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        return stats?.LikesCount ?? 0;
    }

    public async Task<Dictionary<string, int>> GetMultipleStoryLikesCountsAsync(List<string> storyIds, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting likes counts for {Count} stories", storyIds.Count);

        var parameters = new QueryParameters<MasterStoryStats>
        {
            Filter = stat => storyIds.Contains(stat.MasterStoryId)
        };

        var (statsList, _) = await statsRepository.GetAllMatchingAsync(parameters, cancellationToken);

        var result = new Dictionary<string, int>();

        foreach (var stat in statsList)
        {
            result[stat.MasterStoryId] = stat.LikesCount;
        }

        // Add entries for stories with no stats
        foreach (var storyId in storyIds)
        {
            if (!result.ContainsKey(storyId))
            {
                result[storyId] = 0;
            }
        }

        return result;
    }

    public async Task IncrementSharesCountAsync(string storyId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Incrementing shares count for story {StoryId}", storyId);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        if (stats is null)
        {
            // Create new stats record
            stats = new MasterStoryStats
            {
                MasterStoryId = storyId,
                RatingsCount = 0,
                RatingsSum = 0,
                LikesCount = 0,
                SharesCount = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.IsAuthenticated ? currentUserService.UserId : null
            };
            await statsRepository.AddAsync(stats, cancellationToken);
            logger.LogInformation("Created new stats record for story {StoryId} with SharesCount=1", storyId);
        }
        else
        {
            // Update existing stats
            stats.SharesCount++;
            stats.UpdatedAt = DateTime.UtcNow;
            stats.UpdatedBy = currentUserService.IsAuthenticated ? currentUserService.UserId : null;

            await statsRepository.UpdateAsync(stats);
            logger.LogInformation("Updated shares count for story {StoryId}: SharesCount={SharesCount}", storyId, stats.SharesCount);
        }
    }

    public async Task<int> GetSharesCountAsync(string storyId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting shares count for story {StoryId}", storyId);

        var stats = await statsRepository.FirstOrDefaultAsync(e => e.MasterStoryId == storyId, cancellationToken);

        return stats?.SharesCount ?? 0;
    }

    public async Task<Dictionary<string, int>> GetMultipleStorySharesCountsAsync(List<string> storyIds, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Getting shares counts for {Count} stories", storyIds.Count);

        var parameters = new QueryParameters<MasterStoryStats>
        {
            Filter = stat => storyIds.Contains(stat.MasterStoryId)
        };

        var (statsList, _) = await statsRepository.GetAllMatchingAsync(parameters, cancellationToken);

        var result = new Dictionary<string, int>();

        foreach (var stat in statsList)
        {
            result[stat.MasterStoryId] = stat.SharesCount;
        }

        // Add entries for stories with no stats
        foreach (var storyId in storyIds)
        {
            if (!result.ContainsKey(storyId))
            {
                result[storyId] = 0;
            }
        }

        return result;
    }
}
