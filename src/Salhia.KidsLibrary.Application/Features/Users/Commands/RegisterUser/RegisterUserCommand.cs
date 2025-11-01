using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<string>
{
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; } 
    public UserType UserType { get; set; }
}
