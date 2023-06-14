using MainApp.BLL.Entities;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class EventController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITrackingService _trackingService;

        public EventController(ILogger logger, ITrackingService trackingService)
        {
            _logger = logger;
            _trackingService = trackingService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            List<Event> events = await _trackingService.GetAll(sortOrder, searchString);

            if (events.Count() == 0)
            {
                return RedirectToAction("EmptyList");
            }

            _logger.Information("Events load sucessfull at at {loginDate}", DateTime.Now);

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            return View(events.OrderByDescending(e => e.CreatedDate).Take(20));
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
                _logger.Warning($"Event with Id {id} doesn't exist!");
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
                    _logger.Warning($"Event with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                _logger.Warning($"Delete event with id {model.Id}");

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
