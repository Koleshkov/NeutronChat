using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeutronChat.Application.Authentication.Commands.Login
{
    public class LoginVallidator : AbstractValidator<LoginCommand>
    {
        public LoginVallidator()
        {
            RuleFor(request => request.Email)
                .EmailAddress()
                .NotNull();

            RuleFor(request => request.Password)
                .NotNull();
        }
    }
}
