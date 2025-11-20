using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Entities;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.LandingPage.Queries.GetLandingPageStats;

public class GetLandingPageStatsQueryHandler(
    IRepository<MasterStory> storyRepository,
    IRepository<MasterStoryStats> statsRepository,
    IRepository<AppUser> userRepository,
    ILogger<GetLandingPageStatsQueryHandler> logger
) : IRequestHandler<GetLandingPageStatsQuery, GetLandingPageStatsQueryResponse>
{
    public async Task<GetLandingPageStatsQueryResponse> Handle(
        GetLandingPageStatsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching landing page stats");

        // Get all stats records to calculate totals
        var allStats = await statsRepository.GetAllAsync(s => true, cancellationToken);
        
        var totalLikes = allStats.Sum(s => s.LikesCount);
        var totalRatings = allStats.Sum(s => s.RatingsCount);
        var totalRatingsSum = allStats.Sum(s => s.RatingsSum);

        // Calculate average rating
        var averageRating = totalRatings > 0
            ? (decimal)totalRatingsSum / totalRatings
            : 0;

        // Count total approved stories
        var totalStories = await storyRepository.CountAsync(
            s => s.ApprovalStatus == ApprovalStatus.Approved, 
            cancellationToken);

        // Count active users (not locked out)
        var activeUsers = await userRepository.CountAsync(
            u => !u.LockoutEnabled || u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow,
            cancellationToken);

        return new GetLandingPageStatsQueryResponse
        {
            AverageRating = Math.Round(averageRating, 2),
            TotalLikes = totalLikes,
            ActiveUsers = activeUsers,
            TotalStories = totalStories
        };
    }
}
