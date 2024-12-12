using TaskApp.Domain.Interfaces;
using TaskApp.Infrastructure.Tools;

namespace TaskApp.Infrastructure.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            // Check if the endpoint allows anonymous access
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            using var scope = serviceProvider.CreateScope();
            var authenticationService = scope.ServiceProvider.GetService<IAuthenticationService>();

            var token = TokenHelper.GetBearerToken(context.Request);
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is required.");
                return;
            }

            // Validate the token using IAuthenticationService
            var userResult = await authenticationService!.Me(token);
            if (!userResult.Success || userResult.Data == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token.");
                return;
            }

            // Store the authenticated user in the context for further use
            context.Items["User"] = userResult.Data;

            await _next(context);
        }
    }
}
