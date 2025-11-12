using System.ComponentModel.DataAnnotations;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Domain.Entities;

public class MasterStory : BaseEntity
{
    public string StoryCategoryId { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }

    // Navigation property
    public StoryCategory StoryCategory { get; set; } = default!;
    public AppUser Author { get; set; } = default!;
    public AppUser? UpdatedByUser { get; set; }

    public ICollection<MediaItem> MediaItems { get; set; } = [];
    public ICollection<StoryComment> Comments { get; set; } = [];
    public ICollection<StoryRating> Ratings { get; set; } = [];

}
