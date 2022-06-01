using NeutronChat.Application.Services;
using NeutronChat.Domain.Configurations;
using NeutronChat.Domain.Models;
using System.Security.Claims;

namespace NeutronChat.Persistance.Services
{
    public class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly AuthenticationConfiguration configuration;
        private readonly TokenGenerator tokenGenarator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration,
            TokenGenerator tokenGenarator)
        {
            this.configuration=configuration;
            this.tokenGenarator=tokenGenarator;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            return tokenGenarator.GenerateToken(
                configuration.AccessTokenSecretKey,
                configuration.Issuer,
                configuration.Audience,
                configuration.AccessTokenExpirationTime,
                claims);
        }
    }
}
