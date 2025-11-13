namespace Salhia.KidsLibrary.Domain.Entities;

public class StoryLike : BaseEntity
{
    public string UserId { get; set; } = default!;
    public string MasterStoryId { get; set; } = default!;

    // Navigation Properties
    public AppUser User { get; set; } = default!;
    public MasterStory MasterStory { get; set; } = default!;
}
