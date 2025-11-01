using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    IUserService userService,
    IMapper mapper
    ) :  IRequestHandler<UpdateUserCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUserRequest = mapper.Map<UpdateUserRequest>(request);

        return await userService.UpdateUserAsync(updateUserRequest, cancellationToken);
    }
}
