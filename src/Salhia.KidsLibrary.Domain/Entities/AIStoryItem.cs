
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Domain.Entities;

public class AIStorySlide : BaseEntity
{
    public int Index { get; set; }
    public string? Title { get; set; }         // Arabic content (short)
    public string Description { get; set; } = default!;  // Arabic full content
    public string ImagePrompt { get; set; } = default!;  // EN prompt for Fal
    public string ImageUrl { get; set; } = default!;     // final CDN URL
    public AIStorySlideStatus Status { get; set; }

    public string AIStoryId { get; set; } = default!;
    public AIStory AIStory { get; set; } = default!;
    
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
