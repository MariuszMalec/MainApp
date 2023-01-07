using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly TrackingService _trackingService;

        private readonly ILogger<AccountController> _logger;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            ApplicationDbContext applicationDbContext, IRepository<ApplicationUser> userRepository, TrackingService trackingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _trackingService = trackingService;
        }


        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                    var user = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Email,
                        Email = model.Email,
                        Created = DateTime.UtcNow,
                        UserRole = "User"
                    };
                    //LogContext.PushProperty("UserName", model.Email);// co to jest??
                    Serilog.Log.Information("Trying to register new user - {userName} at {registrationDate}", model.Email, DateTime.Now);
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");//TODO tutaj zapisuje do aktualnej bazy a nie do fasady przy tescie, dlaczego?!
                        //await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                        Serilog.Log.Information("User {userName} has been registered successfully at {registrationDate}", model.Email, DateTime.Now);
                        var myEvent = await _trackingService.InsertEvent(ActivityActions.register, this.HttpContext, model.Email);//TODO przez to nie moge testowac!
                        await _trackingService.Insert(myEvent);
                        return RedirectToAction("Login");

                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(model);
                    }
            }
            //LogContext.PushProperty("UserName", model.Email);
            Serilog.Log.Information("Registration of the user - {userName} failed at {registrationDate}", model.Email, DateTime.Now);
            ModelState.AddModelError("", "Invalid Register.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (model.Email == null)
            {
                ModelState.AddModelError("", "Invalid ID or Password");

                Serilog.Log.Information("login attempt failed for the user - {userName} at {loginDate}", model.Email, DateTime.Now);
                return View(model);
            }

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    Serilog.Log.Information("User {userName} logged in successfully at {loginDate}", model.Email, DateTime.UtcNow);
                    var myEvent = await _trackingService.InsertEvent(ActivityActions.loggin, this.HttpContext, model.Email);
                    await _trackingService.Insert(myEvent);
                    return RedirectToAction("Index", "Home");

                }
                else if (result.IsLockedOut)
                {
                    Serilog.Log.Error("User {email} account locked!!", model.Email);
                    _logger.LogError("User {email} account locked!!", model.Email);
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt");
                    Serilog.Log.Error($"invalid login attempt for user {model.Email}");
                    _logger.LogWarning($"invalid login attempt for user {model.Email}");
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Invalid ID or Password");

            Serilog.Log.Information("login attempt failed for the user - {userName} at {loginDate}", model.Email, DateTime.Now);
            return View(model);
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
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

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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

        //[HttpGet]
        //[Route("/AccessDenied")]
        //public ActionResult AccessDenied()
        //{
        //    return LocalRedirect("/Account/AccessDenied");
        //}

        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"User logout at {DateTime.Now}");
            Serilog.Log.Information($"User logout at {DateTime.Now}");
            var userEmail = this.HttpContext.User.Identity.Name;
            var myEvent = await _trackingService.InsertEvent(ActivityActions.logout, HttpContext, userEmail);
            await _trackingService.Insert(myEvent);
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
