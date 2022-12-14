using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using Tracking.Authentication.ApiKey;
using Tracking.Context;
using Tracking.Models;
using Tracking.Repositories;
using Tracking.Services;
using System.Net;
using System.Net.Security;
using Tracking.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

//to musi byc dla core6
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;
var connectionString = configuration.GetConnectionString("Default");
builder.Services.AddDbContext<MainApplicationContext>(o => o.UseNpgsql(connectionString));

builder.Services.AddTransient<IRepositoryService<Trainer>, TrainerService>();

builder.Services.AddTransient<IRepositoryService<User>, UserService>();

builder.Services.AddTransient<IRepositoryService<Event>, TrackingService>();

builder.Services.AddDbContext<MainApplicationContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(sharedOptions =>
    {
        sharedOptions.DefaultScheme = ApiKeyAuthenticationOptions.AuthenticationScheme;
    })
    .AddApiKey<ApiKeyAuthenticationService>(options => configuration.Bind("ApiKeyAuth", options));

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Tracking ApiKey", Version = "v1" });
    option.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid apikey",
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="ApiKey"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MainApplicationContext>();
    //var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();
            if (context.Database.IsRelational())
            {
                if (context.Database.IsRelational())
                {
                    context?.Database.Migrate();
                    //TrainerSeed.SeedTrainer(context);
                    TrainerSeed.SeedTrainers(context);
                }
            }
            else
            {
                //TODO nie ralacyjna baza danych np memory msql do testow
                SeedData.SeedTrainer(context);
                SeedData.SeedUser(context);
                SeedData.SeedEvent(context);
            }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Info API");
    });
}

//app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }