//I used ethereal.com to create fake email to tests

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace SendEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("tina.heidenreich27@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("tina.heidenreich27@ethereal.email"));
            email.Subject = "M.M test";
            email.Body = new TextPart(TextFormat.Html) { Text = body};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);//smtp.gmail.com , smtp.office365.com
            smtp.Authenticate("tina.heidenreich27@ethereal.email", "K8sxNh7hdYjSJABGtZ");
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

            return Ok("Email was send");

        }
    }
}
