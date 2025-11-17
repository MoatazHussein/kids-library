using System.Security.Claims;
using Salhia.KidsLibrary.Application.Common.Interfaces.Security;

namespace Salhia.KidsLibrary.API.Middlewares;

public class UserLockoutCheckMiddleware(ILogger<UserLockoutCheckMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Only check for authenticated users
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var userService = context.RequestServices.GetRequiredService<IUserService>();
                var isDisabled = await userService.IsUserDisabledAsync(userId);

                if (isDisabled)
                {
                    logger.LogWarning("Disabled user {UserId} attempted to access {Path}", userId, context.Request.Path);
                    
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Your account has been disabled. Please contact an administrator.");
                    return;
                }
            }
        }

        await next.Invoke(context);
    }
}
