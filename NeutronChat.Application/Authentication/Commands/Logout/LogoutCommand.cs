using MediatR;

namespace NeutronChat.Application.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}
