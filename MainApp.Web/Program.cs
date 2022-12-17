using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MainApp.BLL.Context;
using MainApp.Web.Services;
using MainApp.BLL;
using MainApp.BLL.Services;
using MainApp.BLL.Repositories;
using MainApp.BLL.Entities;
using Microsoft.Net.Http.Headers;
using MainApp.Web.Middleware;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Security;
using System.Net.Http;

public class ProgramMVC
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        //To trzeba dodac!! aby zadzialalo Configuration!! Sqlite
        // IConfiguration Configuration;
        // Configuration = builder.Configuration;
        // builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Default")));

        //to musi byc dla core6 postgres
        ConfigurationManager configuration = builder.Configuration;
        IWebHostEnvironment environment = builder.Environment;
        var connectionString = configuration.GetConnectionString("Default");
        builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(connectionString));

        //Services configuration

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddTransient<TrackingService>();
        builder.Services.AddTransient<IPersonService, UserService>();
        builder.Services.AddTransient<ITrainersService, TrainersService>();
        builder.Services.AddTransient<EmailService>();
        builder.Services.AddHttpClient();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireDigit = false;
            options.User.RequireUniqueEmail = false;
            options.Lockout.MaxFailedAccessAttempts = 3;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
        })
        .AddRoles<ApplicationRoles>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddHttpContextAccessor();

        //how to solve problem with ssl! only linux!?
        builder.Services.AddHttpClient("Tracking").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
               ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

            });

        builder.Services.AddHttpClient("Tracking", client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:7001/");
                        client.Timeout = new TimeSpan(0, 0, 30);
                        client.DefaultRequestHeaders.Add(
                            HeaderNames.Accept, "application/json");
                        client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
                    });

        builder.Services.AddAuthorization();

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<UserManager<ApplicationUser>>();
        //builder.Services.AddScoped<RoleManager<ApplicationRoles>>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();

            if (context.Database.IsRelational())
            {
                if (context.Database.IsRelational())
                {
                    context?.Database.Migrate();
                    await SeedData.SeedUser(context, userManager, roleManager);
                }
            }
            else
            {
                //TODO nie ralacyjna baza danych np memory msql do testow
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseMiddleware<MyExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization(); ;

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}