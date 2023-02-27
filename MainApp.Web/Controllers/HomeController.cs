using MainApp.BLL.Context;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QC = Microsoft.Data.SqlClient;

namespace MainApp.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext applicationDbContext = null)
        {
            _logger = logger;
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index(LoginView model)
        {
            var defaultprovider = _configuration["DatabaseProvider"];
            if (model.ProviderName == null)
                model.ProviderName = defaultprovider;
            var provider = model.ProviderName;

            provider= (string)TempData["Provider"];

            //ViewBag.Test = provider;

            return View(model);
        }

        public IActionResult Edit(LoginView model)
        {
            string provider = "Nie wybrano!";
            int number;
            bool success = int.TryParse(model.ProviderName, out number);
            if (success)
            {
                provider = Enum.GetName(typeof(Provider), number);
            }
            else
            {
                provider = model.ProviderName;
            }

            var connectionString = _configuration.GetConnectionString(provider);

            //_applicationDbContext.Database.CanConnect();

            //using (var connection = new QC.SqlConnection(
            //        connectionString
            //    ))
            //{
            //    connection.Open();
            //    //connection.ChangeDatabase("MainApp");
            //    //_applicationDbContext.Database.GetDbConnection().ChangeDatabase("");
            //    _logger.LogWarning($"Connected to {strProvider} successfully.");
            //}

            //ViewBag.Test = provider;
            //ViewData["Provider"] = provider;
            TempData["Provider"] = provider;

            _logger.LogWarning($"Connected to {provider} successfully.");

            return RedirectToAction("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
