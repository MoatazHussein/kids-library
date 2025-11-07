namespace Salhia.KidsLibrary.Domain.Entities;

public class MasterStoryStats : BaseEntity
{
    public string MasterStoryId { get; set; } = string.Empty;
    public int RatingsCount { get; set; }
    public int RatingsSum { get; set; }
    
    // Navigation property
    public MasterStory MasterStory { get; set; } = default!;
}
