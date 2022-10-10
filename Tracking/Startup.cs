using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Context;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tracking", Version = "v1" });
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

            if (env.IsDevelopment() && !env.IsEnvironment("Mariusz"))
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tracking v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
