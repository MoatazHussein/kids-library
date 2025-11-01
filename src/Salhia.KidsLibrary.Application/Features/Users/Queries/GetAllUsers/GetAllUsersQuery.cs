using MediatR;
using Salhia.KidsLibrary.Application.Common;
using Salhia.KidsLibrary.Application.Common.Models;
using Salhia.KidsLibrary.Application.Common.Dtos.Users;

namespace Salhia.KidsLibrary.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<PagedResult<UserDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}


