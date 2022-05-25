using SendEmail.API.Models;

namespace SendEmail.API.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request);
    }
}
