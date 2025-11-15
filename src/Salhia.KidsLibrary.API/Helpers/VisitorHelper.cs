namespace Salhia.KidsLibrary.API.Helpers;

public static class VisitorHelper
{
    private const string AnonCookieName = "story_anon_id";

    public static string GetVisitorKey(HttpContext httpContext)
    {
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirst("sub")?.Value
                         ?? httpContext.User.Identity!.Name!;
            return $"user:{userId}";
        }

        if (!httpContext.Request.Cookies.TryGetValue(AnonCookieName, out var anonId)
            || string.IsNullOrWhiteSpace(anonId))
        {
            anonId = Guid.NewGuid().ToString("N");

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMonths(6)
            };

            httpContext.Response.Cookies.Append(AnonCookieName, anonId, options);
        }

        return $"anon:{anonId}";
    }
}
