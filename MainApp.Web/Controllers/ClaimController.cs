using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Web.Controllers
{
    public class ClaimController : Controller
    {
        // GET: ClaimController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClaimController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClaimController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaimController/Create
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

        // GET: ClaimController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClaimController/Edit/5
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

        // GET: ClaimController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClaimController/Delete/5
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
