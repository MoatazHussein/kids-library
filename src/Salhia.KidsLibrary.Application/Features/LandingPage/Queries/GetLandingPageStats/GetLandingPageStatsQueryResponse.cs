namespace Salhia.KidsLibrary.Application.Features.LandingPage.Queries.GetLandingPageStats;

public class GetLandingPageStatsQueryResponse
{
    public decimal AverageRating { get; set; }
    public int TotalLikes { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalStories { get; set; }
}
