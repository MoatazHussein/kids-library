namespace Salhia.KidsLibrary.Application.Features.MasterStories.Queries.GetMasterStories;

public class GetMasterStoriesQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string StoryCategoryId { get; set; } = string.Empty;
    public string? StoryCategoryTitle { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsApproved { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public string? AuthorName { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
