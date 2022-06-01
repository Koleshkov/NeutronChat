using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeutronChat.Application.Services;
using NeutronChat.Domain.Models;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.UpdateToken
{
    public class UpdateTokenCommandHandler : IRequestHandler<UpdateTokenCommand, LoginResponse>
    {
        private readonly IAccessTokenGenerator accessTokenGenerator;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly IRefreshTokenValidator refreshTokenValidator;
        private readonly IAppDbContext appDbContext;
        private readonly UserManager<User> userManager;

        public UpdateTokenCommandHandler(IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenValidator refreshTokenValidator,
            IAppDbContext appDbContext,
            UserManager<User> userManager)
        {
            this.accessTokenGenerator=accessTokenGenerator;
            this.refreshTokenGenerator=refreshTokenGenerator;
            this.refreshTokenValidator=refreshTokenValidator;
            this.appDbContext=appDbContext;
            this.userManager=userManager;
        }

        public async Task<LoginResponse> Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
        {
            var isValidRefreshToken = refreshTokenValidator.Validate(request.RefreshToken);

            if (!isValidRefreshToken)
            {
                throw new Exception("Invalid refresh token");
            }

            var refreshTokenTemp =
                await appDbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token==request.RefreshToken, cancellationToken);

            if (refreshTokenTemp==null)
            {
                throw new Exception("Invalid refresh token");
            }

            appDbContext.RefreshTokens.Remove(refreshTokenTemp);
            await appDbContext.SaveChangesAsync(cancellationToken);

            var user = await userManager.FindByIdAsync(refreshTokenTemp.UserId.ToString());

            if (user==null)
            {
                throw new Exception("User not found.");
            }

            var accessToken = accessTokenGenerator.GenerateToken(user);
            var refreshToken = refreshTokenGenerator.GenerateToken();

            refreshTokenTemp = new()
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
