//I used ethereal.com to create fake email to tests

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SendEmail.API.Models;
using SendEmail.API.Services;

namespace SendEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmailDto reqeuest)
        {
            await _emailService.SendEmail(reqeuest);
            return Ok("Email was send");
        }
    }
}
