using MainApp.BLL.Entities;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class EmailController : Controller
    {
        private EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: EmailController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmailController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmailController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmailController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Email model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isSend = await _emailService.CreateEmail(model);

            if (isSend == null)
            {
                Serilog.Log.Warning($"Email was not send!");
                if (model.Body == null)
                    return BadRequest("Email was not send! Body is empty");
                if (model.Subject == null)
                    return BadRequest("Email was not send! Subject is empty");
                if (model.To == null)
                    return BadRequest("Email can't be empty!");
                if (isSend == null)
                    return BadRequest("Email can't be send! Check internet connection");
            }

            Serilog.Log.Information("Send mail at date {date}", DateTime.Now);
            return RedirectToAction(nameof(EmailWasSendCorrect));
        }

        public ActionResult EmailWasSendCorrect()
        {
            return View();
        }

        // GET: EmailController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmailController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmailController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmailController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
