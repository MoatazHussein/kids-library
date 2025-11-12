using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<string>
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public UserType UserType { get; set; }
}
