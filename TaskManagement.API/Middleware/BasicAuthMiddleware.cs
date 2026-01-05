using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;

namespace TaskManagement.API.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
    HttpContext context,
    IAuthService authService)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Allow public endpoints
            if (path == "/" || path.StartsWith("/swagger") || path.StartsWith("/api/auth"))
            {
                await _next(context);
                return;
            }

            // Protect everything else
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization header missing");
                return;
            }

            var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);

            if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            var username = credentials[0];
            var password = credentials[1];

            User user = await authService.ValidateCredentialsAsync(username, password);

            if (user is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }

    }
}
