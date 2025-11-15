using MediatR;
using Salhia.KidsLibrary.Domain.Enums;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommand : IRequest
{
    public string Email { get; set; } = default!;
    public UserRole NewRole { get; set; }
}
