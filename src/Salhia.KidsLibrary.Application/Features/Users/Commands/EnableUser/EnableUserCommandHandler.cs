using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.EnableUser;

public class EnableUserCommandHandler(
    IUserService userService,
    ILogger<EnableUserCommandHandler> logger
) : IRequestHandler<EnableUserCommand>
{
    public async Task Handle(EnableUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Enabling user {UserId}", request.UserId);

        var success = await userService.EnableUserAsync(request.UserId, cancellationToken);
        
        if (!success)
        {
            throw new NotFoundException("User", request.UserId);
        }

        logger.LogInformation("User {UserId} has been enabled successfully", request.UserId);
    }
}
