using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private readonly TrackingService _trackingService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        public EventController(ILogger<EventController> logger, IHttpClientFactory httpClientFactory, TrackingService trackingService)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
            _trackingService = trackingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> Index()
        {
            List<Event> events = await _trackingService.GetAll();
            if (events.Count() == 0)
            {
                return RedirectToAction("EmptyList");
            }
            return View(events.OrderByDescending(e => e.CreatedDate).Take(20));

        }

        // POST: UserController/Delete/5
        [HttpPost("DeleteAllEvents")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllEvents([FromBody] List<Event> events)
        {
            try
            {
                var check = await _trackingService.DeleteAllEvents();

                if (check == false)
                {
                    return new ContentResult()
                    {
                        Content = "Not found events!"
                    };
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult<Event>> Delete(int id)
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var model = await _trackingService.GetEventById(id, userEmail, this.HttpContext);
            if (model == null)
            {
                _logger.LogWarning($"Event with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Event model)
        {
            try
            {
                var check = await _trackingService.DeleteEvent(id, model, this.HttpContext);

                if (check == false)
                {
                    _logger.LogWarning($"Event with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                _logger.LogWarning($"Delete event with id {model.Id}");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EmptyList(int id)
        {
            ViewBag.Id = id;
            return View();
        }

    }
}
