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
            return View(events.OrderByDescending(e => e.CreatedDate).Take(20));
        }

        // POST: UserController/Delete/5
        [HttpGet("DeleteAllEvents")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAllEvents()
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

        public ActionResult DeleteEvemts()
        {

            return View();
        }


    }
}
