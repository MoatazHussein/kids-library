using System.ComponentModel.DataAnnotations;

namespace Salhia.KidsLibrary.Domain.Entities;

public class MasterStory : BaseEntity
{
    [Required]
    public string StoryCategoryId { get; set; } = default!;

    [Required]
    public string Title { get; set; } = default!;

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }
    public bool IsApproved { get; set; }

    // Navigation property
    public StoryCategory StoryCategory { get; set; } = default!;
    public AppUser Author { get; set; } = default!;
    public AppUser? UpdatedByUser { get; set; }

    public ICollection<MediaItem> MediaItems { get; set; } = [];
    public ICollection<StoryComment> Comments { get; set; } = [];

}
