using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<List<Event>>> Index(string sortOrder)
        {
            List<Event> events = await _trackingService.GetAll();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var sortedEvents = from s in events
                               select s;

            switch (sortOrder)
            {
                case "name_desc":
                    sortedEvents = sortedEvents.OrderByDescending(s => s.Action);
                    break;
                case "Date":
                    sortedEvents = sortedEvents.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    sortedEvents = sortedEvents.OrderByDescending(s => s.CreatedDate);
                    break;
                default:
                    sortedEvents = sortedEvents.OrderBy(s => s.Action);
                    break;
            }

            if (events.Count() == 0)
            {
                return RedirectToAction("EmptyList");
            }
            return View(sortedEvents.Take(20));

        }

        [HttpGet("Event/DeleteAllEvents")]
        public IActionResult DeleteAllEvents()
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost("Event/RemoveAllEvents")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAllEvents()
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
        [Authorize(Roles = "Admin")]
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
