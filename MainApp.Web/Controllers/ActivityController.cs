using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class ActivityController : Controller
    {
        private readonly TrackingService _trackingService;

        // GET: ActivityController
        public ActivityController(TrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        public async Task<IActionResult> Index()
        {
            List<Event> events = await _trackingService.GetAll();

            if (events.Count() == 0)
            {
                return RedirectToAction("EmptyList");
            }

            var sums = events.GroupBy(x => x.Action)
                .ToDictionary(x => x.Key, x => x.Select(y => y.UserId).Sum());

            return View(new ActivityStatisticsView() { ActivitySums = sums});

        }

        // GET: ActivityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ActivityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ActivityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ActivityController/Edit/5
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

        // GET: ActivityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ActivityController/Delete/5
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

        public ActionResult EmptyList()
        {
            return View();
        }
    }
}
