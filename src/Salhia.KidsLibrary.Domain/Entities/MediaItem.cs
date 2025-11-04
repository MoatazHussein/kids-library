using System.ComponentModel.DataAnnotations;

namespace Salhia.KidsLibrary.Domain.Entities;

public class MediaItem : BaseEntity
{
    public string MasterStoryId { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string Url { get; set; } = default!;

    // Navigation properties
    public MasterStory MasterStory { get; set; } = default!;
    
    // User navigation properties
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }
}
