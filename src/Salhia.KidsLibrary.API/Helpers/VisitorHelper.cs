namespace Salhia.KidsLibrary.API.Helpers;

public static class VisitorHelper
{
    public static (string visitorKey, string? userId) GetVisitorKey(HttpContext httpContext)
    {
        // 1) Logged-in user
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirst("sub")?.Value
                         ?? httpContext.User.Identity!.Name!;
            return ($"user:{userId}", userId);
        }

        // 2) Anonymous: from header sent by Vue
        var anonId = httpContext.Request.Headers["X-Visitor-Id"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(anonId))
        {
            // Fallback – should rarely happen
            anonId = Guid.NewGuid().ToString("N");
        }

        return ($"anon:{anonId}", null);
    }
}

