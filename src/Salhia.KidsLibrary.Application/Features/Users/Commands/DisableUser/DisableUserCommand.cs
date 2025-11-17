using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.DisableUser;

public class DisableUserCommand : IRequest
{
    public string UserId { get; set; } = default!;
}
