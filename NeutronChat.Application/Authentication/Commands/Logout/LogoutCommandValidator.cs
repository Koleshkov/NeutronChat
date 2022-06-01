using FluentValidation;

namespace NeutronChat.Application.Authentication.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(request => request.UserId)
                .NotNull();
        }
    }
}
