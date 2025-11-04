using System.ComponentModel.DataAnnotations;

namespace Salhia.KidsLibrary.Domain.Entities;

public class StoryCategory : BaseEntity
{
    [Required]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    // Navigation property
    public ICollection<MasterStory> MasterStories { get; set; } = [];
}
