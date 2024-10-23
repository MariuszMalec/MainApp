using MainApp.BLL.Entities;
using MainApp.BLL.Exceptions;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRepositoryService<ApplicationRoles> _roleService;
        private readonly ILogger _logger;

        public RoleController(IRepositoryService<ApplicationRoles> roleService, ILogger logger = null)
        {
            _roleService = roleService;
            _logger = logger;
        }

        // GET: RoleController
        public async Task<ActionResult> Index()
        {
            var models = await _roleService.GetAll();
            _logger.Information("Get Roles at {registrationDate}", DateTime.Now);
            return View(models);
        }

        // GET: RoleController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _roleService.GetById(id);
            if (model == null)
            {
                _logger.Warning($"Not found role with {id}");
                return NotFound($"Not found role with {id}");
                //return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // GET: RoleController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ApplicationRoles model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model == null)
                {
                    _logger.Warning($"404 Brak roli!");
                    return NotFound("404 Brak roli!");
                }

                var check = await _roleService.Insert(model);
                if (check == false)
                {
                    _logger.Warning($"Role can't be created!");
                    return NotFound("404 Brak roli!");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }


        }

        // GET: RoleController/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationRoles>> Edit(int id)
        {
            var model = await _roleService.GetById(id);
            if (model == null)
            {
                //throw new NotFoundException("Role not found!");
                //return NotFound("Role not found!");
                return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ApplicationRoles model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //var userEmail = this.HttpContext.User.Identity.Name;
                var check = await _roleService.Update(id, model);

                if (check == false)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationRoles>> Delete(int id)
        {
            var model = await _roleService.GetById(id);

            if (model.NormalizedName == "ADMIN")
            {
                _logger.Error($"Role Admin can't be deleted!");
                //return Content($"Admin can't be deleted!");
                return RedirectToAction("AccessDenied", "Account");
            }

            if (model == null)
            {
                _logger.Warning($"Role with Id {id} doesn't exist!");
                return NotFound($"Not found role with {id}");
                //return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, ApplicationRoles model)
        {
            try
            {
                var check = await _roleService.Delete(id, model);

                if (check == false)
                {
                    _logger.Warning($"Role with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                var userEmail = this.HttpContext.User.Identity.Name;
                _logger.Error($"Role {model.Name} delete with id {id} at {DateTime.Now}");

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
