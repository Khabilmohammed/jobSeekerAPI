using jobSeeker.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace jobSeeker.Middlewares
{
    public class BlockedUserMiddleware
    {
        private readonly RequestDelegate _next;
        public BlockedUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null && user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Access denied. Your account is deactivated.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
