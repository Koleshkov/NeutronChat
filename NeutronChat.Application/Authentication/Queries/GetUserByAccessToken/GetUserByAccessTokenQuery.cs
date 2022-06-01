using MediatR;
using NeutronChat.Domain.Models;

namespace NeutronChat.Application.Authentication.Queries.GetUserByAccessToken
{
    public class GetUserByAccessTokenQuery : IRequest<User?>
    {
        public string Token { get; set; } = "";
    }
}
