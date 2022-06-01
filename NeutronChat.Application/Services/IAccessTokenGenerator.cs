using NeutronChat.Domain.Models;

namespace NeutronChat.Application.Services
{
    public interface IAccessTokenGenerator
    {
        string GenerateToken(User user);
    }
}
