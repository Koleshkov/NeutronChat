
using FluentValidation;

namespace NeutronChat.Application.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(request => request.Code)
                .NotNull();

            RuleFor(request => request.Email)
                .EmailAddress()
                .NotNull();
        }
    }
}
