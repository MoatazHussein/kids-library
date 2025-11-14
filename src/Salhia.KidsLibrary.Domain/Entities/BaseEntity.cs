using System.ComponentModel.DataAnnotations;

namespace Salhia.KidsLibrary.Domain.Entities;
public class BaseEntity
{
    [Key]
    [MaxLength(26)] 
    public string Id { get; private set; } = Ulid.NewUlid().ToString();
    
    [MaxLength(26)]
    public string? CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [MaxLength(26)]
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
