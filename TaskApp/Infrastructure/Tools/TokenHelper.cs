namespace TaskApp.Infrastructure.Tools
{
    public static class TokenHelper
    {
        public static string? GetBearerToken(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                const string bearerPrefix = "Bearer ";
                var token = authorizationHeader.ToString();
                if (token.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return token[bearerPrefix.Length..].Trim();
                }
            }

            return null;
        }
    }
}
