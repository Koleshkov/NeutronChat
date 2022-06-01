using Microsoft.EntityFrameworkCore;
using NeutronChat.Domain.Models;

namespace NeutronChat.Application.Services
{
    public interface IAppDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
