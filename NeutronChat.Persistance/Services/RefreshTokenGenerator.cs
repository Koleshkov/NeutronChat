using NeutronChat.Application.Services;
using NeutronChat.Domain.Configurations;

namespace NeutronChat.Persistance.Services
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration configuration;
        private readonly TokenGenerator tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator)
        {
            this.configuration=configuration;
            this.tokenGenerator=tokenGenerator;
        }

        public string GenerateToken()
        {
            return tokenGenerator.GenerateToken(
                configuration.RefreshTokenSecretKey,
                configuration.Issuer,
                configuration.Audience,
                configuration.RefreshTokenExpirationTime);
        }
    }
}
