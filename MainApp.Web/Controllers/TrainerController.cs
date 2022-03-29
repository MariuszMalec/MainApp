using MainApp.BLL.Models;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ILogger<TrainerController> _logger;
        private TrainersService _trainerService;


        public TrainerController(ILogger<TrainerController> logger, TrainersService trainerService)
        {
            _logger = logger;
            _trainerService = trainerService;

        }

        // GET: TrainerController
        public async Task<ActionResult<List<TrainerView>>> Index()
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            List<TrainerView> trainers = await _trainerService.GetAll(userEmail, this.HttpContext);
            _logger.LogInformation("Sciagam dane z bazy danych API...");
            return View(trainers);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult<TrainerView>> Details(int id)
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var model = await _trainerService.GetTrainerById(id, userEmail, this.HttpContext);
            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            _logger.LogInformation($"User {userEmail} sprawdza dane uzytkowniaka o id {id}");
            return View(model);
        }

        // GET: TrainerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserControlle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TrainerView model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var check = await _trainerService.CreateTrainer(model, this.HttpContext);
                _logger.LogInformation($"Create new trainer with id {model.Id} at {DateTime.Now}");

                if (check == false)
                {
                    _logger.LogWarning($"Trainer can't be created, email exist yet!");
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult<TrainerView>> Edit(int id)
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var model = await _trainerService.GetTrainerById(id, userEmail, this.HttpContext);
            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TrainerView model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var check = await _trainerService.EditTrainer(id, model, this.HttpContext);
                _logger.LogInformation($"Edit trainer with id {model.Id} at {DateTime.Now}");

                if (check == false)
                {
                    _logger.LogWarning($"Trainer with Id {model.Id} doesn't exist!");
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult<TrainerView>> Delete(int id)
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var model = await _trainerService.GetTrainerById(id, userEmail, this.HttpContext);
            if (model == null)
            {
                _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, TrainerView model)
        {
            try
            {
                var check = await _trainerService.DeleteTrainer(id, model, this.HttpContext);

                if (check == false)
                {
                    _logger.LogWarning($"Trainer with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                _logger.LogWarning($"Delete trainer with id {model.Id}");

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
