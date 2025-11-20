using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public string UserId { get; set; } = default!;
}
