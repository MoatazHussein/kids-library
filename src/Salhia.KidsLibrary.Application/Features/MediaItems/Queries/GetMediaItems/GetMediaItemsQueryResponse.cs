namespace Salhia.KidsLibrary.Application.Features.MediaItems.Queries.GetMediaItems;

public class GetMediaItemsQueryResponse
{
    public string Id { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
    
    public string CreatedBy { get; set; } = string.Empty;
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public string? UpdatedByUserName { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
