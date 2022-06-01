using MediatR;
using Microsoft.AspNetCore.Identity;
using NeutronChat.Application.Services;
using NeutronChat.Domain.Models;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<User> userManager;
        private readonly IAccessTokenGenerator accessTokenGenerator;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly IAppDbContext appDbContext;

        public LoginCommandHandler(UserManager<User> userManager,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IAppDbContext appDbContext)
        {
            this.userManager=userManager;
            this.accessTokenGenerator=accessTokenGenerator;
            this.refreshTokenGenerator=refreshTokenGenerator;
            this.appDbContext=appDbContext;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user==null)
            {
                throw new Exception($"{request.Email} didn`t register.");
            }

            bool isCorrectPassword = await userManager.CheckPasswordAsync(user, request.Password);

            if (!isCorrectPassword)
            {
                throw new Exception($"Incorrect password.");
            }

            var accessToken = accessTokenGenerator.GenerateToken(user);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenTemp = new()
            {
                Token = refreshToken,
                UserId = user.Id
            };

            await appDbContext.RefreshTokens.AddAsync(refreshTokenTemp, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            { 
                AccessToken=accessToken, 
                RefreshToken=refreshToken 
            };
        }
    }
}
