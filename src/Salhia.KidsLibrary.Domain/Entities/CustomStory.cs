using System.ComponentModel.DataAnnotations;

namespace Salhia.KidsLibrary.Domain.Entities;

public class CustomStory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    // Navigation properties
    public ICollection<CustomStoryItem> CustomStoryItems { get; set; } = [];

    // User navigation properties
    public AppUser CreatedByUser { get; set; } = default!;
    public AppUser? UpdatedByUser { get; set; }
}
