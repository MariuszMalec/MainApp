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
        public async Task<ActionResult> Index()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");
            var models = await _trainerRepository.GetAll();
            return View(models);
        }

        // GET: TrainerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            _logger.LogInformation($"Sciagam dane uzytkowniak o id {id}");
            var model = await _trainerRepository.GetById(id);

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

                await _trainerRepository.Insert(model);
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
            var model = await _trainerRepository.GetById(id);
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

                await _trainerRepository.Update(model);
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
            var model = await _trainerRepository.GetById(id);
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
                await _trainerRepository.Delete(model);
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
