using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.EnableUser;

public class EnableUserCommand : IRequest
{
    public string UserId { get; set; } = default!;
}
