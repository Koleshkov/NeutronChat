using NeutronChat.Domain.Models;
using NeutronChat.Domain.Requests;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.BlazorClient.Services
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<RegisterResponse?> RegisterUserAsync(RegisterRequest request);
        Task<LoginResponse?> UpdateTokenAsync(string refreshToken);
        Task SendConfirmationCodeAsync(string email);
        Task<User?> GetUserByAccessTokenAsync(string token); 
        Task LogoutAsync();
    }
}
