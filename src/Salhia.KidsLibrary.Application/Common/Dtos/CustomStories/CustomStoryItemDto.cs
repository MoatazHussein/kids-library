namespace Salhia.KidsLibrary.Application.Common.Dtos.CustomStories;

public class CustomStoryItemDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomStoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    
    public string CreatedBy { get; set; } = string.Empty;
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public string? UpdatedBy { get; set; }
    public string? UpdatedByUserName { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
