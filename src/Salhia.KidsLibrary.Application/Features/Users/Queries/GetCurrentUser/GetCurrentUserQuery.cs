using System.Security.Claims;
using MediatR;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;

namespace Salhia.KidsLibrary.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery(ClaimsPrincipal user) : IRequest<UserDto>
{
    public ClaimsPrincipal User { get; set; } = user;
}
