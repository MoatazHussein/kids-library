using MediatR;
using Microsoft.Extensions.Logging;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.ChangeUserRole;

public class ChangeUserRoleCommandHandler(
    IUserService userService,
    ILogger<ChangeUserRoleCommandHandler> logger
) : IRequestHandler<ChangeUserRoleCommand>
{
    public async Task Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Changing user role for {Email} to {NewRole}", request.Email, request.NewRole);

        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException("User", request.Email);
        }

        // Get current roles
        var currentRoles = await userService.GetRolesAsync(user.Id, cancellationToken);
        
        logger.LogInformation("User {Email} currently has {RoleCount} role(s): {Roles}", 
            request.Email, currentRoles.Count, string.Join(", ", currentRoles));

        // Remove all current roles
        foreach (var role in currentRoles)
        {
            var removed = await userService.RemoveFromRoleAsync(request.Email, role, cancellationToken);
            if (!removed)
            {
                logger.LogWarning("Failed to remove role {Role} from user {Email}", role, request.Email);
            }
        }

        // Assign new role
        var assigned = await userService.AddToRoleAsync(user.Id, request.NewRole.ToString(), cancellationToken);
        if (!assigned)
        {
            throw new InvalidOperationException($"Failed to assign role '{request.NewRole}' to user '{request.Email}'");
        }

        // Update UserType to match the new role (UserRole and UserType enums have matching values)
        var userTypeUpdated = await userService.UpdateUserTypeAsync(user.Id, (int)request.NewRole, cancellationToken);
        if (!userTypeUpdated)
        {
            logger.LogWarning("Failed to update UserType for user {Email} after role change", request.Email);
        }

        logger.LogInformation("Successfully changed role and user type for {Email} to {NewRole}", request.Email, request.NewRole);
    }
}
