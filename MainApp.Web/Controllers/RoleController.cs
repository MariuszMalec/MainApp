using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MainApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleController(ILogger<RoleController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var models = new List<ApplicationUserRoleView> () {
                new ApplicationUserRoleView () {
                    Id = _context.Users.Select(u=>u.Id).First(),
                    FirstName = _context.Users.Select(u=>u.FirstName).First(),
                    LastName = _context.Users.Select(u=>u.LastName).First(),
                    UserId = _context.UserRoles.Select(u=>u.UserId).First(),
                    RoleId = _context.UserRoles.Select(u=>u.RoleId).First(),
                }
            };
            return View(models);
        }

        // GET: UserController/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<ApplicationUserRoleView>> Edit(int id)
        {
            //var userEmail = this.HttpContext.User.Identity.Name;

            var user = await _context.Users.FindAsync(id);

            var model = new ApplicationUserRoleView () {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = _context.UserRoles.Where(u=>u.UserId == id).Select(u=>u.UserId).First(),
                RoleId = _context.UserRoles.Where(u=>u.UserId == id).Select(u=>u.RoleId).First(),
            };
            
            if (model == null)
            {
                Serilog.Log.Information($"user with Id {id} doesn't exist!");
                return RedirectToAction("EmptyList");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ApplicationUserRoleView model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                //var userEmail = this.HttpContext.User.Identity.Name;

                var updateModel = new ApplicationUserRoleView () {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserId = model.UserId,
                    RoleId = model.RoleId
                };

                var user = await _context.Users.FindAsync(model.UserId);

                var role = await _context.UserRoles.FindAsync(model);//TODO jak wyciagnac roleuser tabele

                _context.UserRoles.Update(new IdentityUserRole<int> () {
                    UserId = model.UserId, 
                    RoleId = model.RoleId});

                await _context.SaveChangesAsync();

                var check = false;//await _trainerService.EditTrainer(id, model, this.HttpContext);
                Serilog.Log.Information("User {userName} edit trainer with id {id} at {date}", model.UserId, model.RoleId,DateTime.Now);

                if (check == false)
                {
                    Serilog.Log.Warning($"user with Id {model.UserId} doesn't exist!");
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("/AccessDenied")]
        public ActionResult AccesDenied(string returnUrl = null)
        {
            return View();
        }

    }
}