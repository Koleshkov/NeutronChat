using FluentValidation;

namespace NeutronChat.Application.Authentication.Commands.GenerateCode
{
    public class GenerateCodeCommandValidator : AbstractValidator<GenerateCodeCommand>
    {
        public GenerateCodeCommandValidator()
        {
            RuleFor(request => request.Email)
                .EmailAddress()
                .NotNull();
        }
    }
}
