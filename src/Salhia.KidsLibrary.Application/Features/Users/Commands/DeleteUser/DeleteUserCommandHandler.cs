using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Constants;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(
    IUserService userService,
    ICurrentUserService currentUserService,
    ILogger<DeleteUserCommandHandler> logger
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user {UserId}", request.UserId);

        // Prevent user from deleting themselves
        var currentUserId = currentUserService.UserId;
        if (currentUserId == request.UserId)
        {
            throw new UnAuthorizedAccessException("You cannot delete your own account");
        }

        // Check if user exists
        var user = await userService.FindByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException("User", request.UserId);
        }

        var success = await userService.DeleteUserAsync(request.UserId, cancellationToken);
        
        if (!success)
        {
            throw new AppException("Failed to delete user");
        }

        logger.LogInformation("User {UserId} has been deleted successfully", request.UserId);
    }
}
