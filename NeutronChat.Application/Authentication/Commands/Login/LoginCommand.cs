using AutoMapper;
using MediatR;
using NeutronChat.Application.Common.Mappings;
using NeutronChat.Domain.Requests;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponse>, IMapWith<LoginRequest>
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoginRequest, LoginCommand>();
        }
    }
}
