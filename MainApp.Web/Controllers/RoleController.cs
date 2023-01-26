using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService<ApplicationRoles> _roleService;

        public RoleController(IRoleService<ApplicationRoles> roleService)
        {
            _roleService = roleService;
        }

        // GET: RoleController
        public async Task<ActionResult> Index()
        {
            var models = await _roleService.GetAll();
            return View(models);
        }

        // GET: RoleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                    return NotFound("404 Brak roli!");

                var check = await _roleService.Insert(model);
                if (check == false)
                {
                    Serilog.Log.Warning($"Role can't be created!");
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoleController/Edit/5
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

        // GET: RoleController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApplicationRoles>> Delete(int id)
        {
            var model = await _roleService.GetById(id);

            if (model.NormalizedName == "ADMIN")
            {
                return Content($"Admin can't be deleted!");
            }

            if (model == null)
            {
                Serilog.Log.Warning($"Role with Id {id} doesn't exist!");
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
                    Serilog.Log.Information($"Rolr with Id {id} doesn't exist!");
                    return RedirectToAction("EmptyList");
                }

                var userEmail = this.HttpContext.User.Identity.Name;
                Serilog.Log.Information($"Role {model.Name} delete with id {id} at {DateTime.Now}");

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
