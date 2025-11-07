
namespace Salhia.KidsLibrary.Application.Common.Dtos.Users;

public class LoginResponseDto
{
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string PhoneNumber  { get; set; } = default!;
    public string Token { get; set; } = default!;
    public int UserTypeValue { get; set; }  
    public string UserTypeName { get; set; } = default!;
    public IReadOnlyList<string> Roles { get; set; } = default!;
}
