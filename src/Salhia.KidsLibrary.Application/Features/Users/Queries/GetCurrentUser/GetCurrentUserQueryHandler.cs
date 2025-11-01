using AutoMapper;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Domain.Exceptions;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;

namespace Salhia.KidsLibrary.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter,
    IUserService userService
) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userService.GetByClaimsPrincipalAsync(
            request.User,
            null,
            cancellationToken);

        if (user == null)
            throw new UnAuthorizedAccessException("User is not authenticated.");

        var dto = mapper.Map<UserDto>(user);

        dto.Roles = await userService.GetUserRolesAsync(user, cancellationToken);

        return timeZoneConverter.ConvertUtcToLocal(dto);
    }
}
