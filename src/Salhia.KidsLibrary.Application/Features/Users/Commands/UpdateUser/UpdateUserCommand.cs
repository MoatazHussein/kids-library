using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Salhia.KidsLibrary.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber
) : IRequest<IdentityResult>;