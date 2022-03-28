using MainApp.BLL.Context;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using MainApp.Web.Middleware;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            var connectionString = Configuration.GetConnectionString("MyDatabase");
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<UserService>();
            services.AddTransient<TrainersService>();
            services.AddTransient<EventService>();
            services.AddHttpClient();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddAuthentication(CookieScheme).
                AddCookie(CookieScheme, options =>
                {
                    options.AccessDeniedPath = "/account/AccessDenied";
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/logout";
                });

            services.AddHttpContextAccessor();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            context.Database.Migrate();

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
