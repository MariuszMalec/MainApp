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
    [Authorize(Roles = "Admin,User")]
    public class TrainerController : Controller
    {
        private ITrainersService _trainerService;

        public TrainerController(ITrainersService trainerService)
        {
            _trainerService = trainerService;

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
                Serilog.Log.Warning("UnAuthorized");
                return RedirectToAction("UnAuthorized");      
            }

            Serilog.Log.Information("Download datas from API...");
            return View(sortedTrainers);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult<TrainerView>> Details(int id)
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var model = await _trainerService.GetTrainerById(id, userEmail, this.HttpContext);
            if (model == null)
            {
                Serilog.Log.Warning($"Trainer with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            Serilog.Log.Information($"User {userEmail} sprawdza dane uzytkowniaka o id {id}");
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
                Serilog.Log.Information("User {userName} create new trainer at {date}", userEmail, DateTime.Now);

                if (check == false)
                {
                    Serilog.Log.Warning($"Trainer can't be created, email exist yet!");
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
                Serilog.Log.Information($"Trainer with Id {id} doesn't exist!");
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
                Serilog.Log.Information("User {userName} edit trainer with id {id} at {date}", userEmail, model.Id,DateTime.Now);

                if (check == false)
                {
                    Serilog.Log.Warning($"Trainer with Id {model.Id} doesn't exist!");
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
                Serilog.Log.Warning($"Trainer with Id {id} doesn't exist!");
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
                    Serilog.Log.Information($"Trainer with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                var userEmail = this.HttpContext.User.Identity.Name;
                Serilog.Log.Information("User {userName} delete trainer with id {id} at {date}", userEmail, model.Id, DateTime.Now);
    
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
