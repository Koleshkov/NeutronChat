using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NeutronChat.Persistance.Services
{
    public class TokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer,
            string audience, double expirationTime,
            IEnumerable<Claim>? claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credintials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(expirationTime),
                credintials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
