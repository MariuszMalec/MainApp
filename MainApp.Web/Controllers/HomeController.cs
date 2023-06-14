﻿using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using static ProgramMVC;
using static System.Net.Mime.MediaTypeNames;
using QC = Microsoft.Data.SqlClient;

namespace MainApp.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IApplicationLifetime _applicationLifetime;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IApplicationLifetime applicationLifetime, SignInManager<ApplicationUser> signInManager = null)
        {
            _logger = logger;
            _configuration = configuration;
            _applicationLifetime = applicationLifetime;
            _signInManager = signInManager;
        }

        public IActionResult Index(LoginView model, string param)
        {

            if (param == string.Empty || param == null)
            {
                model.ProviderName = _configuration["Provider"];
                model.RememberMe = true;
                model.Email = "";
                model.Password = "";
                _logger.LogInformation($"Wybrany provider {model.ProviderName}");
                return View(model);
            }

            var viewModelFromEdit = JsonConvert.DeserializeObject<LoginView>(param);
            string provider = "Nie wybrano!";
            int number;
            bool success = int.TryParse(viewModelFromEdit.ProviderName, out number);
            if (success)
            {
                provider = Enum.GetName(typeof(Provider), number);
            }
            else
            {
                provider = viewModelFromEdit.ProviderName;
            }

            var startProvider = _configuration["Provider"];//TODO from program.cs

            if (startProvider != provider)//TODO tutaj wstawic nowy z edycji provider i wystartowac apke
            {
                model.ProviderName = provider;
                model.RememberMe= true;
                model.Email = "";
                model.Password = "";
                _logger.LogInformation($"Wybrany provider {model.ProviderName}");
                return View(model);
            }

            _logger.LogInformation($"Wybrany provider {model.ProviderName}");
            return View(model);

        }

        public async Task<IActionResult> Edit(LoginView model)
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

            var defaultProvider = _configuration["Provider"];

            if (defaultProvider != provider)
            {
                //TODO tutaj od nowa odpalic apke z wyborem providera!! jak to zrobic??                

                _signInManager.SignOutAsync();
                //TempData["Provider"] = provider;

                model.RememberMe = true;
                model.Email = "";
                model.Password = "";

                ViewData["Modelek"] = JsonConvert.SerializeObject(model);
                //return RedirectToAction(nameof(Index));

                //_applicationLifetime.StopApplication();//TODO stop app

                //System.Web.HttpRuntime.UnloadAppDomain();

                //AppDomain.Unload();

                //await ProgramMVC.Main(new string[] { provider });

                //Process currentProcess = Process.GetCurrentProcess();
                //string applicationPath = currentProcess.MainModule.FileName;
                //Process.Start(applicationPath);
                //currentProcess.Kill();
                //await ProgramMVC.Main(new string[] { provider });

                return Content("change to new provider , how ???????????");//TODO Jak stad z restartowac applikacje!

                //return new EmptyResult();

                //return RedirectToAction("Index", "Home", new { param = JsonConvert.SerializeObject(model) });


            }

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
