using Salhia.KidsLibrary.Application.Common.Models.Dashboard;
using Salhia.KidsLibrary.Application.Features.Dashboard.Common.Enums;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Common.Interfaces;

/// <summary>
/// Repository for optimized dashboard analytics queries
/// </summary>
public interface IDashboardRepository
{
    /// <summary>
    /// Get trending stories with growth metrics using optimized SQL query
    /// </summary>
    /// <param name="recentFrom">Start of recent period</param>
    /// <param name="comparisonFrom">Start of comparison period (for growth calculation)</param>
    /// <param name="top">Number of top stories to return</param>
    /// <param name="categoryId">Optional category filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of trending stories with calculated metrics</returns>
    Task<List<TrendingStoryData>> GetTrendingStoriesAsync(
        DateTime recentFrom,
        DateTime comparisonFrom,
        int top,
        string? categoryId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get top performing stories based on various metrics using optimized SQL query
    /// </summary>
    /// <param name="periodFrom">Start of analysis period (null for all-time)</param>
    /// <param name="sortBy">Metric to sort by</param>
    /// <param name="top">Number of top stories to return</param>
    /// <param name="categoryId">Optional category filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of top stories with engagement metrics</returns>
    Task<List<TopStoryData>> GetTopStoriesAsync(
        DateTime? periodFrom,
        SortBy sortBy,
        int top,
        string? categoryId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get detailed engagement metrics for a specific story using optimized SQL query
    /// </summary>
    /// <param name="storyId">Story ID</param>
    /// <param name="periodFrom">Start of analysis period</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Comprehensive engagement data including daily breakdown</returns>
    Task<StoryEngagementData> GetStoryEngagementAsync(
        string storyId,
        DateTime periodFrom,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get comprehensive user statistics and activity metrics using optimized SQL query
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="periodFrom">Start of analysis period</param>
    /// <param name="includeTopCategories">Include top categories breakdown</param>
    /// <param name="includeDailyActivity">Include daily activity timeline</param>
    /// <param name="topCategoriesLimit">Number of top categories to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User statistics with optional top categories and daily activity</returns>
    Task<UserStatsData> GetUserStatsAsync(
        string userId,
        DateTime periodFrom,
        bool includeTopCategories = true,
        bool includeDailyActivity = true,
        int topCategoriesLimit = 5,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get dashboard overview with comprehensive metrics using optimized SQL queries
    /// </summary>
    /// <param name="period">Dashboard period for analysis</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dashboard overview data with counts, engagement metrics, and rates</returns>
    Task<DashboardOverviewData> GetDashboardOverviewAsync(
        DashboardPeriod period,
        CancellationToken cancellationToken = default);
}
