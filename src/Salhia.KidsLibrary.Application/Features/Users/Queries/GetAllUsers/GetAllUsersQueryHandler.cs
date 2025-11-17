using System.Linq.Expressions;
using AutoMapper;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;
using Salhia.KidsLibrary.Application.Common.Interfaces;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Domain.Entities;
using MediatR;

namespace Salhia.KidsLibrary.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(
    IUserService userService,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
) : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var includes = new Expression<Func<AppUser, object>>[]
            {
            };

        var pagedUsers = await userService.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchPhrase,
            request.UserType,
            includes,
            cancellationToken);

        var userDtos = new List<UserDto>();
        foreach (var user in pagedUsers.Items)
        {
            var dto = mapper.Map<UserDto>(user);
            dto.Roles = await userService.GetUserRolesAsync(user, cancellationToken);
            dto.IsActive = !await userService.IsUserDisabledAsync(user.Id);
            userDtos.Add(dto);
        }

        var result = new PagedResult<UserDto>(userDtos, pagedUsers.TotalItemsCount, request.PageSize, request.PageNumber);


        return timeZoneConverter.ConvertUtcToLocal(result); 
    }
}


