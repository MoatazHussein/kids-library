using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Domain.Entities;

public class MasterStory : BaseEntity
{
    public string StoryCategoryId { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Content { get; set; }

    public string? CoverImageUrl { get; set; }

    public MediaType MediaType { get; set; }
    public string MediaUrl { get; set; } = default!;    
    
    public int? PublishYear { get; set; }
    
    public string AuthorName { get; set; } = default!;
    
    public ApprovalStatus ApprovalStatus { get; set; }

    // Navigation property
    public StoryCategory StoryCategory { get; set; } = default!;
    public AppUser? CreatedByUser { get; set; }
    public AppUser? UpdatedByUser { get; set; }

    public ICollection<StoryComment> Comments { get; set; } = [];
    public ICollection<StoryRating> Ratings { get; set; } = [];
    public ICollection<StoryLike> Likes { get; set; } = [];
    public ICollection<StoryShare> Shares { get; set; } = [];
    public ICollection<StoryViewSession> ViewSessions { get; set; } = [];
    public MasterStoryStats? MasterStoryStats { get; set; } 

}
