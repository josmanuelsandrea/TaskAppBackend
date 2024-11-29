using System.Security.Claims;

namespace TaskApp.Interfaces.Auth
{
    public interface IJwtService
    {
        public string GenerateJwtToken(string userId);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
