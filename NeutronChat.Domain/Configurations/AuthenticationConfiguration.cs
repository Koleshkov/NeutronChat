namespace NeutronChat.Domain.Configurations
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecretKey { get; set; } = "";
        public string RefreshTokenSecretKey { get; set; } = "";
        public double AccessTokenExpirationTime { get; set; }
        public double RefreshTokenExpirationTime { get; set; }
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
    }
}
