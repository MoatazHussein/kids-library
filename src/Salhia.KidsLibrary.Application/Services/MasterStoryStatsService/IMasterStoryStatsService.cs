namespace Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;

public interface IMasterStoryStatsService
{
    // Write operations - Ratings
    Task IncrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default);
    Task UpdateRatingAsync(string storyId, int oldRating, int newRating, CancellationToken cancellationToken = default);
    Task DecrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default);
    
    // Write operations - Likes
    Task UpdateLikesCountAsync(string storyId, int increment, CancellationToken cancellationToken = default);
    
    // Write operations - Shares
    Task IncrementSharesCountAsync(string storyId, CancellationToken cancellationToken = default);
    
    // Read operations
    Task<(int RatingsCount, decimal? AverageRating)> GetStoryRatingStatsAsync(string storyId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, (int RatingsCount, decimal? AverageRating)>> GetMultipleStoryRatingStatsAsync(List<string> storyIds, CancellationToken cancellationToken = default);
    Task<int> GetLikesCountAsync(string storyId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetMultipleStoryLikesCountsAsync(List<string> storyIds, CancellationToken cancellationToken = default);
    Task<int> GetSharesCountAsync(string storyId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetMultipleStorySharesCountsAsync(List<string> storyIds, CancellationToken cancellationToken = default);

    Task<int> GetStoryViewsCountAsync(string storyId);
    Task<Dictionary<string, int>> GetMultipleStoryViewsCountsAsync(IEnumerable<string> storyIds);
}
