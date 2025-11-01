using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Common.Dtos.Users;
public class RegisterUserRequest
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserType UserType { get; set; }

}
