
namespace NeutronChat.Application.Services
{
    public interface IEmailSender
    {
        Task SendAsync(string mailto, string subject, string message);
    }
}
