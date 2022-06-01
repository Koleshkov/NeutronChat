using MediatR;

namespace NeutronChat.Application.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest
    {
        public string Email { get; set; } = "";
        public string Code { get; set; } = "";

    }
}
