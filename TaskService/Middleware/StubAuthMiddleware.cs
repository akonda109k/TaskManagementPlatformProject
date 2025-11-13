using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;

namespace TaskService.Middleware
{
    public class StubAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public StubAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check for stub token in Authorization header
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                var payload = token.Substring("Bearer ".Length);
                // For stub, payload is just a JSON string with username and role
                try
                {
                    var user = JsonSerializer.Deserialize<StubUserPayload>(payload);
                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, user.Role)
                        };
                        var identity = new ClaimsIdentity(claims, "Stub");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
                catch (JsonException ex)
                {
                    // Log the invalid token error for diagnostics
                    var logger = context.RequestServices.GetService<ILogger<StubAuthMiddleware>>();
                    logger?.LogWarning(ex, "Invalid stub token in Authorization header: {Token}", token);
                }
            }
            await _next(context);
        }

        private class StubUserPayload
        {
            public string Username { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }

    public static class StubAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseStubAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StubAuthMiddleware>();
        }
    }
}
