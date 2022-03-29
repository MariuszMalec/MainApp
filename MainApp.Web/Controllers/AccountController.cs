using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Services;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly TrackingService _trackingService;
        private ApplicationDbContext _context;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        private readonly UserToApiService _userToApiService;


        public AccountController(ILogger<AccountController> logger, IAccountService accountService, UserService userService, ApplicationDbContext context, IHttpClientFactory httpClientFactory, UserToApiService userToApiService, TrackingService trackingService)
        {
            _logger = logger;
            _accountService = accountService;
            _userService = userService;
            _context = context;
            this.httpClientFactory = httpClientFactory;
            _userToApiService = userToApiService;
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

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterView request)
        {
            if (ModelState.IsValid)
            {
                //TODO: add scope
                using var log = _logger.BeginScope("UserEmailCheckInRegistration");
                _logger.LogDebug("Checking if user exists {userEmail}", request.Email);
                var userCheck = await _userService.GetByEmail(request.Email);

                if (userCheck == null)
                {
                    var user = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Role = _context.Roles.Find(2)
                    };

                    var password = Base64EncodeDecode.Base64Encode(request.Password);
                    user.PasswordHash = password;

                    if (user != null)
                    {
                        _logger.LogInformation($"User {request.Email} created successfully");

                        //await _userToApiService.CreateUser(request, user);//wyslanie do Api usera ale chyba nie potrzebnie

                        await _userService.Insert(user);

                        var myEvent = await _trackingService.InsertEvent(ActivityActions.register, this.HttpContext, request.Email);
                        await _trackingService.Insert(myEvent);

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        _logger.LogWarning($"Adding new user {request.Email} not successfully");
                        ModelState.AddModelError("message", "Error with adding new user");
                        return View(request);
                    }
                }
                else
                {
                    _logger.LogWarning($"User with this email {request.Email} already exists");
                    ModelState.AddModelError("message", "Email already exists.");
                    return View(request);
                }
            }
            return View(request);

        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var authResult = await ValidateLogin(userName, password);
            // Normally Identity handles sign in, but you can do it directly
            if (authResult.Success == true)
            {
                var claims = new List<Claim>//TODO rolename!!
                {
                    new Claim("user", authResult.UserName),
                    new Claim("role", authResult.RoleName)
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                _logger.LogInformation($"User {userName} login successfully");

                var myEvent = await _trackingService.InsertEvent(ActivityActions.loggin, this.HttpContext, userName);
                await _trackingService.Insert(myEvent);

                return RedirectToAction("Index", "Account", new { email = userName, role = authResult.RoleName }); // TODO dodac index w account!

            }
            else
            {
                _logger.LogWarning($"invalid login attempt for user {userName}");
            }

            return View("UserIsNotRegistered", "Account");
        }

        private async Task<LoginResult> ValidateLogin(string userName, string password)
        {
            var authResult = await _accountService.ValidateUser(userName, password);
            return authResult;
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
        public async Task<IActionResult> Logout()
        {
            var userEmail = this.HttpContext.User.Identity.Name;
            var myEvent = await _trackingService.InsertEvent(ActivityActions.logout, this.HttpContext, userEmail);
            await _trackingService.Insert(myEvent);
            await HttpContext.SignOutAsync();
            _logger.LogInformation($"User {userEmail} logout successfully");
            return Redirect("/");
        }
    }
}
