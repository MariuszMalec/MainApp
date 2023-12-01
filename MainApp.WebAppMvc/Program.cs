using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MainApp.WebAppMvc.Data;
using MainApp.WebAppMvc.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MainAppWebAppMvcContextConnection") ?? throw new InvalidOperationException("Connection string 'MainAppWebAppMvcContextConnection' not found.");

builder.Services.AddDbContext<MainAppWebAppMvcContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MainAppWebAppMvcUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<MainAppWebAppMvcContext>();

builder.Services.AddHttpClient();//TODO add to use IHttpClientFactory

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MainAppWebAppMvcContext>();//TODO tutaj powinien wejsc do OnConfiguring 
    var takeConfiguration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    if (context.Database.IsRelational())
    {
        if (context.Database.IsRelational())
        {
            context.Database.EnsureCreated();
            //context?.Database.Migrate();
            //SeedData.SeedAdmin(context);
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();//TODO TO MUSI BYC JAK CHCEM UZYWAC IDENTITY/PAGE

app.Run();
