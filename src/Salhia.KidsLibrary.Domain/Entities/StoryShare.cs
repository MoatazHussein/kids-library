using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Domain.Entities;

public class StoryShare : BaseEntity
{
    public string? UserId { get; set; } 
    public string MasterStoryId { get; set; } = default!;
    public SharePlatform Platform { get; set; }
    public string? IpAddress { get; set; } 

    // Navigation Properties
    public AppUser? User { get; set; }
    public MasterStory MasterStory { get; set; } = default!;
}
