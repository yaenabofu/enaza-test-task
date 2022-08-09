using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using web_api.Models;

namespace web_api.Interfaces
{
    public interface IJwtToken
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<AuthenticatedResponse> Refresh(TokenApiModel tokenApiModel);
        Task<bool> Revoke(string login);
    }
}
