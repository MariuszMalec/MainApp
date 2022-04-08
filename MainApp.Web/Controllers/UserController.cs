using AutoMapper;
using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.ExtentionsMethod;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserService userService, IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }
        // GET: UserController1
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAll();
            if (!users.Any())
            {
                return View("No users!");
            }

            //var roleLoggedUser = ExtentionsMethod.GetRoles((ClaimsIdentity)User.Identity);//tylko zalogowanego

            foreach (var item in users)
            {
                //userId = await _userManager.GetUserIdAsync(item);

                var userByMail = await _userManager.FindByEmailAsync(item.Email);

                var roleUser = await _context.UserRoles.Where(x=>x.UserId == userByMail.Id).Select(x=>x.RoleId).FirstOrDefaultAsync();

                var roleName = await _context.Roles.Where(x=>x.Id == roleUser).Select(x=>x.Name).FirstOrDefaultAsync();

                if (roleName != null)
                {
                    item.UserRole = roleName;
                }
            }

            var model = _mapper.Map<List<UserView>>(users);

            return View(model);
        }

        // GET: UserController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController1/Create
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

        // GET: UserController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController1/Edit/5
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

        // GET: UserController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController1/Delete/5
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
