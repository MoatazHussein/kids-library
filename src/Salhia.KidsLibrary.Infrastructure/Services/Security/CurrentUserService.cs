using System.Security.Claims;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;
using Salhia.KidsLibrary.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Salhia.KidsLibrary.Infrastructure.Services.Security;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
               throw new UnAuthorizedAccessException("User is not authenticated.");
            }

            return userId;

        }
    }

    public string UserName
    {
        get
        {
            var userName = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnAuthorizedAccessException("User is not authenticated or username is missing.");
            }

            return userName;
        }
    }

    public string Email
    {
        get
        {
            var userEmail = httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new UnAuthorizedAccessException("User is not authenticated or username is missing.");
            }

            return userEmail;
        }
    }
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles =>
        httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?
            .Select(c => c.Value) ?? Enumerable.Empty<string>();

    public IEnumerable<Claim> Claims =>
        httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();

    public bool IsInRole(string role)
    {
        if (string.IsNullOrEmpty(role))
            return false;

        return httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public bool HasClaim(string claimType, string claimValue)
    {
        if (string.IsNullOrEmpty(claimType))
            return false;

        return httpContextAccessor.HttpContext?.User?.HasClaim(claimType, claimValue) ?? false;
    }
}
