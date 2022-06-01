using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NeutronChat.Domain.Configurations;
using NeutronChat.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NeutronChat.Application.Authentication.Queries.GetUserByAccessToken
{
    public class GetUserByAccessTokenQueryHandler : IRequestHandler<GetUserByAccessTokenQuery, User?>
    {
        private readonly AuthenticationConfiguration configuration;
        private readonly UserManager<User> userManager;

        public GetUserByAccessTokenQueryHandler(IOptions<AuthenticationConfiguration> configuration, UserManager<User> userManager)
        {
            this.configuration=configuration.Value;
            this.userManager=userManager;
        }

        public async Task<User?> Handle(GetUserByAccessTokenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(configuration.AccessTokenSecretKey);



                TokenValidationParameters tokenValidationParameters = new()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = configuration.Issuer,
                    ValidAudience = configuration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken securityToken;
                var principle = tokenHandler.ValidateToken(request.Token, tokenValidationParameters, out securityToken);

                JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;



                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principle.FindFirst("Id")?.Value;

                    var user = await userManager.Users.Where(u => u.Id.ToString() == userId).FirstOrDefaultAsync();

                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }
}
