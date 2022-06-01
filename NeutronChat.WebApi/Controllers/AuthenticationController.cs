using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NETCore.MailKit.Core;
using NeutronChat.Domain.Requests;
using NeutronChat.Application.Authentication.Commands.Register;
using NeutronChat.Application.Authentication.Commands.GenerateCode;
using NeutronChat.Application.Authentication.Commands.ConfirmEmail;
using NeutronChat.Application.Authentication.Commands.Login;
using NeutronChat.Domain.Models;
using NeutronChat.Application.Authentication.Queries.GetUserByAccessToken;
using NeutronChat.Application.Authentication.Commands.UpdateToken;
using NeutronChat.Application.Authentication.Commands.Logout;

namespace NeutronChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public AuthenticationController(IMediator mediator, IEmailService emailService,
            IMapper mapper)
        {
            this.mediator=mediator;
            this.emailService=emailService;
            this.mapper=mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = mapper.Map<RegisterCommand>(request);

            var response = await mediator.Send(command);

            await SendConfirmationCode(command.Email);

            return Ok(response);
        }

        [HttpPost("SendConfirmationCode")]
        private async Task<IActionResult> SendConfirmationCode(string email)
        {
            var code = await mediator.Send(new GenerateCodeCommand { Email=email });

            var callbackUrl = Url.Action("ConfirmEmail", "Authentication", new { email, code },
                        protocol: HttpContext.Request.Scheme);

            await emailService.SendAsync(email, "Подтверждение регистрации",
            $"<H1>Подтвердите регистрацию, перейдя по <a href='{callbackUrl}'>ссылке</a>.</H1>",
            true);

            return Ok();
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var command = new ConfirmEmailCommand()
            {
                Email=email,
                Code = code
            };

            await mediator.Send(command);

            return Content($"Email: {email} успешно подтвержден!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = mapper.Map<LoginCommand>(request);

            var response = await mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("GetUserByAccessToken")]
        public async Task<IActionResult> GetUserByAccessToken([FromBody] string accessToken)
        {
            GetUserByAccessTokenQuery query = new()
            {
                Token = accessToken
            };

            var response = await mediator.Send(query);

            if (response==null) return BadRequest("Bad token");

            return Ok(response);
        }

        [HttpPost("UpdateToken")]
        public async Task<IActionResult> UpdateToken([FromBody] string refreshToken)
        {
            var command = new UpdateTokenCommand() { RefreshToken= refreshToken };

            var response = await mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if (!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            LogoutCommand logoutRequest = new()
            {
                UserId = userId
            };

            await mediator.Send(logoutRequest);

            return NoContent();
        }
    }
}
