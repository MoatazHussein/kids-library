namespace Salhia.KidsLibrary.Application.Common.Dtos.Users;

public class UserInfoDto
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    
    public string FullName => string.IsNullOrWhiteSpace(LastName) 
        ? FirstName 
        : $"{FirstName} {LastName}";
}
