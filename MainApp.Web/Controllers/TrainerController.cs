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
        private readonly ILogger<TrainerController> _logger;
        private TrainerService _trainserService;

        public TrainerController(ILogger<TrainerController> logger, TrainerService trainserService)
        {
            _logger = logger;
            _trainserService = trainserService;
        }

        // GET: TrainerController
        public async Task<ActionResult> Index()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");
            var models = await _trainserService.GetAll();
            return View(models);
        }

        // GET: TrainerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            _logger.LogInformation($"Sciagam dane uzytkowniak o id {id}");
            var model = await _trainserService.GetById(id);

            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        // GET: TrainerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Trainer model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _trainserService.Insert(model);
                _logger.LogInformation($"Create new trainer with id {model.Id}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _trainserService.GetById(id);
            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: TrainerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Trainer model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _trainserService.Update(model);
                _logger.LogInformation($"Edit trainer with id {model.Id}");


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _trainserService.GetById(id);
            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: TrainerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Trainer model)
        {
            try
            {
                await _trainserService.Delete(model);
                _logger.LogWarning($"Delete trainer with id {model.Id}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
