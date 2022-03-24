using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ILogger<TrainerController> _logger;
        private TrainerService _trainserService;
        private UserService _userService;
        private EventService _eventService;

        public TrainerController(ILogger<TrainerController> logger, TrainerService trainserService, EventService eventService, UserService userService)
        {
            _logger = logger;
            _trainserService = trainserService;
            _eventService = eventService;
            _userService = userService;
        }

        // GET: TrainerController
        public async Task<ActionResult> Index()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");
            await _eventService.InsertEvent(ActivityActions.ViewTrainers, this.HttpContext);
            var models = await _trainserService.GetAll();
            return View(models);
        }

        // GET: TrainerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string userEmail = await _eventService.InsertEvent(ActivityActions.detail, this.HttpContext);
            _logger.LogInformation($"User {userEmail} sprawdza dane uzytkowniaka o id {id}");

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

                string userEmail = await _eventService.InsertEvent(ActivityActions.create, this.HttpContext);

                if (userEmail == model.Email)
                    _logger.LogWarning($"Trainer can't be created, email exist yet!");
                    return RedirectToAction("Create");

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

                string userEmail = await _eventService.InsertEvent(ActivityActions.edit, this.HttpContext);
                if (userEmail == model.Email)
                    _logger.LogWarning($"Trainer can't be edit, email exist yet!");
                return RedirectToAction("Edit");

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
                string userEmail = await _eventService.InsertEvent(ActivityActions.delete, this.HttpContext);
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
