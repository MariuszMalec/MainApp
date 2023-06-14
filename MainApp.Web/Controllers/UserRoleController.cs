using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepositoryService<ApplicationUserRoleView> _roleService;
        // GET: RoleController
        public UserRoleController(ILogger<UserRoleController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRepositoryService<ApplicationUserRoleView> roleService)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _roleService = roleService;
        }

        public async Task<ActionResult> Index()
        {

            var models = await _roleService.GetAll();

            if (models == null)
                return Content("models is null!");

            _logger.LogInformation("Get users role at {loginDate}", DateTime.Now);
            return View(models);
        }

        // GET: RoleController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var model = await _roleService.GetById(id);
            if (model == null)
            {
                _logger.LogWarning($"Not found user role with {id}");
                return NotFound($"Not found user role with {id}");
                //return RedirectToAction("EmptyList");
            }
            _logger.LogInformation($"Get details of role {id}");
            return View(model);
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
        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<ApplicationUserRoleView>> Edit(int id)
        {
            //var userEmail = this.HttpContext.User.Identity.Name;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"Not found user with {id}");
                return NotFound($"Not found user with {id}");
            }

            var model = new ApplicationUserRoleView()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = _context.UserRoles.Where(u => u.UserId == id).Select(u => u.UserId).First(),
                RoleId = _context.UserRoles.Where(u => u.UserId == id).Select(u => u.RoleId).First(),
                UserRole = user.UserRole
            };

            if (model == null)
            {
                _logger.LogWarning($"user with Id {id} doesn't exist!");
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

                var user = await _context.Users.FindAsync(model.UserId);

                var userRoles = await _userManager.GetRolesAsync(user);

                var userId = _context.UserRoles.Where(r => r.UserId == model.Id).Select(r => r.UserId).FirstOrDefault();
                var roleId = _context.UserRoles.Where(r=>r.UserId == model.Id).Select(r => r.RoleId).FirstOrDefault();

                //check exist roles
                var checkRoleExist = _context.Roles.Any(r => r.Id == model.RoleId);
                if (checkRoleExist == false)
                {
                    _logger.LogWarning("Role doesn't exist!");
                    return Content("Role doesn't exist!");
                }

                _context.UserRoles.Remove(new IdentityUserRole<int>()
                {
                    UserId = userId,
                    RoleId = roleId
                });

                //set name from selected enum
                var modelRoleName = Enum.GetName(typeof(Roles), int.Parse(model.UserRole));
                var newRoleId = _context.Roles.Where(r => r.Name == modelRoleName).Select(r => r.Id).FirstOrDefault();

                _context.Add(new IdentityUserRole<int>()
                {
                    UserId = model.UserId,
                    RoleId = newRoleId
                });

                //TODO zmien UserRole name w applicationuser!        
                var newUser = await _context.Users.FindAsync(model.Id);
                newUser.UserRole = modelRoleName;

                await _context.SaveChangesAsync();

                _logger.LogInformation("User {userName} edit trainer with id {id} at {date}", model.UserId, model.RoleId, DateTime.Now);

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
