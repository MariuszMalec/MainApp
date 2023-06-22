using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tracking.Context;

namespace MainAppIntegtratedMvcTests
{
    public class TestingMainAppWebAppFactory<TEntryPoint> : WebApplicationFactory<ProgramMVC> where TEntryPoint : ProgramMVC
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            //builder.ConfigureServices(services =>
            //{
            //    var dbContextOptions = services
            //        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            //    services.Remove(dbContextOptions);

            //    services
            //     .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryUsersTest"));//TODO Czemu nie dziala to!!

            //});
            //builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.

            builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryUsersTest");
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    try
                    {
                        appContext.Database.EnsureCreated();
                        //await SeedData(appContext);
                    }
                    catch (Exception ex)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            });
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }
            var role = new ApplicationRoles()
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            context.AddRange(role);
            await context.SaveChangesAsync();
        }

    }
}
