using MediatR;

namespace NeutronChat.Application.Authentication.Commands.GenerateCode
{
    public class GenerateCodeCommand : IRequest<string>
    {
        public string Email { get; set; } = "";
    }
}
