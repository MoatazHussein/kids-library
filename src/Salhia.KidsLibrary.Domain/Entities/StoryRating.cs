namespace Salhia.KidsLibrary.Domain.Entities;

public class StoryRating : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string MasterStoryId { get; set; } = string.Empty;
    public int Rating { get; set; } // 1-5 stars
    
    // Navigation properties
    public AppUser User { get; set; } = default!;
    public MasterStory MasterStory { get; set; } = default!;
}
