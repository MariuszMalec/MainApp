using SendEmail.API.Models;

namespace SendEmail.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailDto request);
    }
}
