using NeutronChat.Domain.Responses;

namespace NeutronChat.Domain.Resposes
{
    public class LoginResponse : BaseResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
