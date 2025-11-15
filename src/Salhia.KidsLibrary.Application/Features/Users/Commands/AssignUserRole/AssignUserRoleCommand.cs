using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public UserRole Role { get; set; }
}