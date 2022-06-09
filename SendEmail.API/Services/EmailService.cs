using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SendEmail.API.Models;

namespace SendEmail.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        IHttpClientFactory _httpClientFactory;
        public EmailService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public IConfiguration Config { get; }

        public async Task SendEmail(EmailDto request)
        {
            //Use SimpleMailTransferProtocol
            HttpClient client = _httpClientFactory.CreateClient();//dodane aby wyciagnac dane z tracking api

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();//remember use reference MailKit.Net.Smtp! More safetly!!

            //jak zrobic np z 2-verification bo teraz jest malo bezpiecznie!

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);//smtp.gmail.com , smtp.office365.com

            var authenticate = smtp.IsAuthenticated;

            if (!authenticate)
            {
                throw new Exception($"Brak autoryzacji!! BoxEmail is securited!! Go to google account, select less security option and switch on it");
            }

            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
