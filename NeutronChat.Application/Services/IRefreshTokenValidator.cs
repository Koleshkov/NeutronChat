namespace NeutronChat.Application.Services
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);
    }
}
