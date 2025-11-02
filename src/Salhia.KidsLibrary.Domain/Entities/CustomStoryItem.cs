
namespace Salhia.KidsLibrary.Domain.Entities;

public class CustomStoryItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public string CustomStoryId { get; set; } = string.Empty;

    // Navigation properties
    public CustomStory CustomStory { get; set; } = null!;
    
    // User navigation properties
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
