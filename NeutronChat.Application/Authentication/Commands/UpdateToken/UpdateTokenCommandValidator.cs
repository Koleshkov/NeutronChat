
using FluentValidation;

namespace NeutronChat.Application.Authentication.Commands.UpdateToken
{
    public class UpdateTokenCommandValidator : AbstractValidator<UpdateTokenCommand>
    {
        public UpdateTokenCommandValidator()
        {
            RuleFor(request => request.RefreshToken)
                .NotNull();
        }
    }
}
