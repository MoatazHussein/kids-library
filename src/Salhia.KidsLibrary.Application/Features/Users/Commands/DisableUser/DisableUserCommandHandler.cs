using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.DisableUser;

public class DisableUserCommandHandler(
    IUserService userService,
    ILogger<DisableUserCommandHandler> logger
) : IRequestHandler<DisableUserCommand>
{
    public async Task Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Disabling user {UserId}", request.UserId);

        var success = await userService.DisableUserAsync(request.UserId, cancellationToken);
        
        if (!success)
        {
            throw new NotFoundException("User", request.UserId);
        }

        logger.LogInformation("User {UserId} has been disabled successfully", request.UserId);
    }
}
