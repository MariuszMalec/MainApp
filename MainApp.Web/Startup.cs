using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using MainApp.Web.Middleware;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace MainApp.Web
{
    public class Startup
    {
        public const string CookieScheme = "CiasteczkaWMojejAplikacji";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connectionString));

            services.AddTransient<TrackingService>();
       
            //do wyrzucenia chyba
            services.AddTransient<UserService>();
            services.AddTransient<TrainersService>();
            services.AddHttpClient();

            services.AddRazorPages();

            services.AddDefaultIdentity<ApplicationUser>(options =>
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

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddHttpContextAccessor();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRoles> roleManager)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context?.Database.Migrate();
                SeedData.SeedClient(context, userManager, roleManager);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            Log.Information("Application is running");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<MyExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
