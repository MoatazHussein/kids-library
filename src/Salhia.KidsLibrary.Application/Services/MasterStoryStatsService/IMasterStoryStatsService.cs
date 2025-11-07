namespace Salhia.KidsLibrary.Application.Services.MasterStoryStatsService;

public interface IMasterStoryStatsService
{
    // Write operations
    Task IncrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default);
    Task UpdateRatingAsync(string storyId, int oldRating, int newRating, CancellationToken cancellationToken = default);
    Task DecrementRatingAsync(string storyId, int ratingValue, CancellationToken cancellationToken = default);
    
    // Read operations
    Task<(int RatingsCount, decimal? AverageRating)> GetStoryRatingStatsAsync(string storyId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, (int RatingsCount, decimal? AverageRating)>> GetMultipleStoryRatingStatsAsync(List<string> storyIds, CancellationToken cancellationToken = default);
}
