using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class TrainerController : Controller
    {
        private readonly IRepository<Trainer> _trainerRepository;
        private readonly ILogger<TrainerController> _logger;
        

        public TrainerController(IRepository<Trainer> trainerRepository, ILogger<TrainerController> logger)
        {
            _trainerRepository = trainerRepository;
            _logger = logger;
        }

        // GET: TrainerController
        public ActionResult Index()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");

            var dataFromBase =_trainerRepository.GetAll();

            return View();
        }

        // GET: TrainerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainerController/Create
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

        // GET: TrainerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainerController/Edit/5
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

        // GET: TrainerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainerController/Delete/5
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
