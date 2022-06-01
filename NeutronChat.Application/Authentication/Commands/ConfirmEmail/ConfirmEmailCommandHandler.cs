using MediatR;
using Microsoft.AspNetCore.Identity;
using NeutronChat.Domain.Models;

namespace NeutronChat.Application.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<User> userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            this.userManager=userManager;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception($"Email \"{request.Email}\" doesn`t exist.");
            }

            if (user.EmailConfirmed)
            {
                throw new Exception($"Email \"{request.Email}\" already confirmed.");
            }

            var result = await userManager.ConfirmEmailAsync(user, request.Code);

            if (!result.Succeeded)
            {
                throw new Exception("Oups, something went wrong.");
            }

            return Unit.Value;
        }
    }
}
