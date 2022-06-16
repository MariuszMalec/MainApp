using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using SendEmail.API.Models;
using System.Net.Http.Headers;

namespace SendEmail.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        IHttpClientFactory _httpClientFactory;
        private const string AppiUrl = "https://localhost:7001/api";
        public EmailService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public IConfiguration Config { get; }

        public async Task SendEmail(EmailDto request)
        {
            //get from other api events
            HttpClient client = _httpClientFactory.CreateClient();//dodane aby wyciagnac dane z tracking api
            var requestEvents = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Tracking");
            requestEvents.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(requestEvents);
            if (!result.IsSuccessStatusCode)
            {
                new List<Event>();
            }
            var content = await result.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            //Use SimpleMailTransferProtocol
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = request.Body };//to co wstawisz w swg

            //wysyla na mail kto sie logowal jako ostatni
            email.Body = new TextPart(TextFormat.Html) { Text = events.Where(x=>x.Action == "loggin").Select(x=>x.Email).LastOrDefault()};

            using var smtp = new SmtpClient();//remember use reference MailKit.Net.Smtp! More safetly!!

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);//smtp.gmail.com , smtp.office365.com

            //var authenticate = smtp.IsAuthenticated;//TODO to przestalo dzialac z dniem 30.05.2022!!
            //if (!authenticate)
            //{
            //    throw new Exception($"Brak autoryzacji!! BoxEmail is securited!! Go to google account, select less security option and switch on it");
            //}

            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
