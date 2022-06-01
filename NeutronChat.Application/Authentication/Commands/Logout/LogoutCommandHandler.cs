
using MediatR;
using Microsoft.EntityFrameworkCore;
using NeutronChat.Application.Services;

namespace NeutronChat.Application.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAppDbContext appDbContext;

        public LogoutCommandHandler(IAppDbContext appDbContext)
        {
            this.appDbContext=appDbContext;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshTokens =  await appDbContext.RefreshTokens
                .Where(t => t.UserId == request.UserId)
                .ToListAsync();

            if (refreshTokens==null)
                throw new Exception("User is logged out.");

            appDbContext.RefreshTokens.RemoveRange(refreshTokens);

            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
