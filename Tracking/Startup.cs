using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Tracking.Authentication.ApiKey;
using Tracking.Context;
using Tracking.Middleware;
using Tracking.Models;
using Tracking.Repositories;
using Tracking.Services;

namespace Tracking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            //sqlite
            //var connectionString = Configuration.GetConnectionString("Default");
            //services.AddDbContext<MainApplicationContext>(o => o.UseSqlite(connectionString));

            //msql
            var connectionString = Configuration.GetConnectionString("Default");
            services.AddDbContext<MainApplicationContext>(o => o.UseSqlServer(connectionString));

            services.AddTransient<IRepositoryService<Trainer>, TrainerService>();

            services.AddTransient<IRepositoryService<User>, UserService>();

            services.AddTransient<IRepositoryService<Event>, TrackingService>();

            services.AddDbContext<MainApplicationContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = ApiKeyAuthenticationOptions.AuthenticationScheme;
            })
            .AddApiKey<ApiKeyAuthenticationService>(options => Configuration.Bind("ApiKeyAuth", options));

            services.AddSwaggerGen(option =>
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainApplicationContext context)
        {
            if (context.Database.IsRelational())
            {
                if (context.Database.IsRelational())
                {
                    context?.Database.Migrate();
                    TrainerSeed.SeedTrainer(context);
                }
            }
            else
            {
                //TODO nie ralacyjna baza danych np memory msql do testow
                SeedData.SeedTrainer(context);
                SeedData.SeedUser(context);
                SeedData.SeedEvent(context);
            }

            if (env.IsDevelopment())
            {
                //TODO aby dzialal test integration musi to byc zakomentowane. Nie mozna uzywac wielu proviederow
                //context?.Database.Migrate();
            }

            if (env.IsEnvironment("Mariusz"))
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tracking v1"));
            }

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
