using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MainApp.Web.Tests
{
    public class TestingMainAppWebAppFactory<TEntryPoint> : WebApplicationFactory<ProgramMVC> where TEntryPoint : ProgramMVC
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

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
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
                await Task.CompletedTask;
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
