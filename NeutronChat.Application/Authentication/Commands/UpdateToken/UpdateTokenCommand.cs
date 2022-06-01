using MediatR;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.UpdateToken
{
    public class UpdateTokenCommand : IRequest<LoginResponse>
    {
        public string RefreshToken { get; set; } = "";
    }
}
