using MainApp.BLL.Models;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    [Authorize(Roles = "Admin,User")]//TODO bez claimow tylko logowanie przez identity
    //[Authorize(Policy = "RequireAdmin")]//TODO dodalem claimy podczas wlasnego slogowania
    public class TrainerController : Controller
    {
        private ITrainersService _trainerService;
        private readonly ILogger<TrainerController> _logger;

        public TrainerController(ITrainersService trainerService, ILogger<TrainerController> logger)
        {
            _trainerService = trainerService;
            _logger = logger;
        }

        // GET: TrainerController
        public async Task<ActionResult<List<TrainerView>>> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var users = this.HttpContext.User;

            var userEmail = this.HttpContext.User.Identity.Name;
            List<TrainerView> trainers = await _trainerService.GetAll(userEmail, this.HttpContext);

            var sortedTrainers = from s in trainers
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    sortedTrainers = sortedTrainers.OrderByDescending(s => s.LastName);
                    break;
                default:
                    sortedTrainers = sortedTrainers.OrderBy(s => s.LastName);
                    break;
            }

            if (!sortedTrainers.Any())
            {
                _logger.LogWarning("UnAuthorized");
                return RedirectToAction("UnAuthorized");      
            }

            _logger.LogInformation("Download trainers from Tracking API...");
            return View(sortedTrainers);
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
                var userEmail = this.HttpContext.User.Identity.Name;

                var check = await _trainerService.CreateTrainer(model, this.HttpContext);
                _logger.LogInformation("User {userName} create new trainer at {date}", userEmail, DateTime.Now);

                if (check == false)
                {
                    _logger.LogWarning($"Trainer can't be created, email exist yet!");
                    return RedirectToAction("EmailExistYet");
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
                var userEmail = this.HttpContext.User.Identity.Name;
                var check = await _trainerService.EditTrainer(id, model, this.HttpContext);
                _logger.LogWarning("User {userName} edit trainer with id {id} at {date}", userEmail, model.Id,DateTime.Now);

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
        [Authorize(Roles = "Admin")]
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

                var userEmail = this.HttpContext.User.Identity.Name;
                _logger.LogWarning("User {userName} delete trainer with id {id} at {date}", userEmail, model.Id, DateTime.Now);
    
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

        public ActionResult EmailExistYet()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }
    }
}
