using AutoMapper;
using MediatR;
using NeutronChat.Application.Common.Mappings;
using NeutronChat.Domain.Models;
using NeutronChat.Domain.Requests;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>, IMapWith<RegisterRequest>
    {

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterRequest, RegisterCommand>();
            profile.CreateMap<User, RegisterResponse>();
        }
    }
}
