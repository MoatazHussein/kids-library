using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.AssignUserRole;


public class AssignUserRoleCommandHandler(IUserService userService) : IRequestHandler<AssignUserRoleCommand, bool>
{
    public async Task<bool> Handle(AssignUserRoleCommand request, CancellationToken ct)
    {
        var user = await userService.FindByEmailAsync(request.Email, ct);

        if (user is null || string.IsNullOrWhiteSpace(request.Email)) return false;

        return await userService.AddToRoleAsync(user.Id, request.RoleName, ct);
    }
}



