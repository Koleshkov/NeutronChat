using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NeutronChat.Domain.Models;
using NeutronChat.Domain.Resposes;

namespace NeutronChat.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public RegisterCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager=userManager;
            this.mapper=mapper;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }

            return mapper.Map<RegisterResponse>(user);
        }
    }
}
