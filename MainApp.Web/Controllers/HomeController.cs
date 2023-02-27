using MainApp.BLL.Context;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
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

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index(LoginView model)
        {
            var defaultprovider = _configuration["DatabaseProvider"];
            if (model.ProviderName == null)
                model.ProviderName = defaultprovider;
            var provider = model.ProviderName;
            return View(model);
        }

        public IActionResult Edit(LoginView model)
        {
            int intProvider = Convert.ToInt32(model.ProviderName);
            string strProvider = Enum.GetName(typeof(Provider), intProvider);

            var connectionString = _configuration.GetConnectionString(strProvider);


            //using (var connection = new QC.SqlConnection(
            //        connectionString
            //    ))
            //{
            //    connection.Open();
            //    _logger.LogWarning($"Connected to {strProvider} successfully.");
            //    //return RedirectToAction("Index");

            



            _logger.LogWarning($"Connected to {strProvider} successfully.");

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
