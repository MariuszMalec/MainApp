using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        // GET: RoleController
        public RoleController(ILogger<RoleController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            var models = users.Select(MapApplicationUserApplicationUserRoleView);//reczne mapowanie na model

            if (models == null)
                return Content("models is null!");

            return View(models);
        }

        private ApplicationUserRoleView MapApplicationUserApplicationUserRoleView(ApplicationUser appUser)
        {
            var roleId = _context.UserRoles.Where(r => r.UserId == appUser.Id).Select(r => r.RoleId).FirstOrDefault();
            return new ApplicationUserRoleView
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                UserId = appUser.Id,
                RoleId = roleId,
                UserRole = _context.Roles.Where(u=>u.Id== roleId).Select(r=>r.Name).FirstOrDefault()
            };
        }

        // GET: RoleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
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

        // GET: UserController/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<ApplicationUserRoleView>> Edit(int id)
        {
            //var userEmail = this.HttpContext.User.Identity.Name;

            var user = await _context.Users.FindAsync(id);

            var model = new ApplicationUserRoleView()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = _context.UserRoles.Where(u => u.UserId == id).Select(u => u.UserId).First(),
                RoleId = _context.UserRoles.Where(u => u.UserId == id).Select(u => u.RoleId).First(),
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

                var updateModel = new ApplicationUserRoleView()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserId = model.UserId,
                    RoleId = model.RoleId
                };

                var user = await _context.Users.FindAsync(model.UserId);

                var userRoles = await _userManager.GetRolesAsync(user);

                var userId = _context.UserRoles.Where(r => r.UserId == model.Id).Select(r => r.UserId).FirstOrDefault();
                var roleId = _context.UserRoles.Where(r=>r.UserId == model.Id).Select(r => r.RoleId).FirstOrDefault();

                //check exist roles
                var checkRoleExist = _context.Roles.Any(r => r.Id == model.RoleId);
                if (checkRoleExist == false)
                {
                    return Content("Role doesn't exist!");
                }

                _context.UserRoles.Remove(new IdentityUserRole<int>()
                {
                    UserId = userId,
                    RoleId = roleId
                });

                _context.Add(new IdentityUserRole<int>()
                {
                    UserId = model.UserId,
                    RoleId = model.RoleId
                });

                //await _userManager.RemoveFromRolesAsync(user, userRoles);//TODO usuniecie role a z applicationuser
                //await _userManager.AddToRoleAsync(user, "Admin");

                await _context.SaveChangesAsync();

                Serilog.Log.Information("User {userName} edit trainer with id {id} at {date}", model.UserId, model.RoleId, DateTime.Now);

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

        // GET: RoleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleController/Delete/5
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
