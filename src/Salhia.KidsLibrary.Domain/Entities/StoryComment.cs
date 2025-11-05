namespace Salhia.KidsLibrary.Domain.Entities;

public class StoryComment : BaseEntity
{
    public string MasterStoryId { get; set; } = default!;

    public string Content { get; set; } = default!;

    // Navigation properties
    public MasterStory MasterStory { get; set; } = default!;
    public AppUser CreatedByUser { get; set; } = default!;
    public AppUser? UpdatedByUser { get; set; }
}
