namespace Salhia.KidsLibrary.Application.Common.Dtos.Users;
public class UpdateUserRequest
{
    public string UserId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
}
