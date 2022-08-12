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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var check = await _emailService.CreateEmail(model);
                Serilog.Log.Information("Send mail at date {date}", DateTime.Now);

                if (check == false)
                {
                    Serilog.Log.Warning($"Email was not send!");
                    return BadRequest("No send email!");
                }              

                return RedirectToAction(nameof(EmailWasSendCorrect));
            }
            catch
            {
                return View();
            }
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
