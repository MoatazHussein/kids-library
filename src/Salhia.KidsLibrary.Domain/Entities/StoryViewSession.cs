namespace Salhia.KidsLibrary.Domain.Entities;
public class StoryViewSession : BaseEntity
{
    public string MasterStoryId { get; set; } = default!;

    public string VisitorKey { get; set; } = default!; // "user:{id}" OR "anon:{id}"

    public string? UserId { get; set; }

    public DateTime LastViewAt { get; set; }
    
    public int ViewCount { get; set; }
}