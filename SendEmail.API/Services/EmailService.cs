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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public EmailService(IConfiguration config, IHttpClientFactory httpClientFactory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public IConfiguration Config { get; }

        public async Task<bool> SendEmail(EmailDto request)
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
            //email.Body = new TextPart(TextFormat.Html) { Text = $"Last logged user was {events.Where(x=>x.Action == "loggin").Select(x=>x.Email).LastOrDefault()}"};
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body }; //TODO narazie test tylko pozniej chcem wyslac eventy logowan

            using var smtp = new SmtpClient();//remember use reference MailKit.Net.Smtp! More safetly!!

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);//smtp.gmail.com , smtp.office365.com

            if (!smtp.IsConnected)
            {
                return false;
            }

            //var authenticate = smtp.IsAuthenticated;//TODO to przestalo dzialac z dniem 30.05.2022!!
            //if (!authenticate)
            //{
            //    throw new Exception($"Brak autoryzacji!! BoxEmail is securited!! Go to google account, select less security option and switch on it");
            //}

            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);

            if (!smtp.IsAuthenticated)
                return false;

            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return true;
        }
    }
}