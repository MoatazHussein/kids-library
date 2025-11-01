using Microsoft.AspNetCore.Identity;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Domain.Entities;

public class AppUser : IdentityUser<string>
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public UserType UserType { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
}
