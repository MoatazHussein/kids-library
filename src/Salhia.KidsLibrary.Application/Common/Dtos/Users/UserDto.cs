namespace Salhia.KidsLibrary.Application.Common.Dtos.Users;

public class UserDto
{
    public string Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public int UserTypeValue { get; set; }
    public string UserTypeName { get; set; } = default!;
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; } 
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = [];


}

