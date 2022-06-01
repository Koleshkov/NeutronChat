using FluentValidation;

namespace NeutronChat.Application.Authentication.Queries.GetUserByAccessToken
{
    public class GetUserByAccessTokenQueryValidator:AbstractValidator<GetUserByAccessTokenQuery>
    {
        public GetUserByAccessTokenQueryValidator()
        {
            RuleFor(request => request.Token)
                .NotNull();
        }
    }
}
