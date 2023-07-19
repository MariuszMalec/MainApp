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
using MainApp.Web.ClaimsFactory;
using MainApp.BLL.Models;
using MainApp.BLL.Enums;
using Microsoft.DotNet.Scaffolding.Shared;
using Serilog.Context;
using Microsoft.Extensions.Hosting.Internal;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Net.Sockets;
using Microsoft.Identity.Client;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using Microsoft.CodeAnalysis.Elfie.Serialization;

public class ProgramMVC
{
    private static CancellationTokenSource _tokenSource = new CancellationTokenSource();
    private static bool _restartRequest;
    private static TcpListener server = null;
    private static bool _selectProviderFromConsole = false;

    public static async Task Main(string[] args)
    {

        if (args.Length > 0)
        {
            //here current Web application start again with new provider
        }

        var builder = WebApplication.CreateBuilder(args);

        ConfigurationManager configuration = builder.Configuration;
        IWebHostEnvironment environment = builder.Environment;

        //-------------------------------------------------------
        //Add serilog from appsetings
        //-------------------------------------------------------
        var conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

        // var connectionstring = "Server = localhost; Port=5432; User Id=mario; Password=mario13; Database=MainAppWebPg;";
        // var logger = new LoggerConfiguration()
        //    //.ReadFrom.Configuration(conf)//TODO czytanie z appsettings
        //    //ponize bez appsetings
        //    .MinimumLevel.Verbose()
        //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        //    .Enrich.FromLogContext()
        //    .WriteTo.Console()
        //    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log/log.txt"), rollingInterval: RollingInterval.Day)
        //    .WriteTo.PostgreSQL(connectionstring, "Logs")
        //    .CreateLogger();
        // builder.Host.UseSerilog();
        // logger.Information("Starting app ...");
        // Log.CloseAndFlush();


        //-------------------------------------------------------
        // -------------- ustalenie providera -------------------
        //-------------------------------------------------------
        var provider = configuration["DatabaseProvider"];//TODO z appsettings.json
        
        if (environment.EnvironmentName == "SqlServer")//TODO zmiana providera gdy wybrane spec. srodowisko
        {
            provider = Provider.SqlServer.ToString();
        }
        if (environment.EnvironmentName == "WinPostgres")//TODO zmiana providera gdy wybrane spec. srodowisko
        {
            provider = Provider.PostgresWin.ToString();
        }
        if (environment.EnvironmentName == "WinMySql")//TODO zmiana providera gdy wybrane spec. srodowisko
        {
            provider = Provider.MySqlWin.ToString();
        }
        if (environment.EnvironmentName == "LinuxMySql")//TODO zmiana providera gdy wybrane spec. srodowisko
        {
            provider = Provider.MySqlLinux.ToString();
        }
        if (environment.EnvironmentName == "LinuxPostgres")//TODO zmiana providera gdy wybrane spec. srodowisko
        {
            provider = Provider.PostgresLinux.ToString();
        }
        //only for unit tests
        if (environment.EnvironmentName == "UnitTests")
        {
            provider = Provider.UnitTests.ToString();
        }

        if (_selectProviderFromConsole)//TODO zmiana providera jesli wybrany inny przez console
        {
            provider = SelectProvider();
            args = new string[] { provider };
            provider = args[0];
        }

        //-------------------------------------------------------
        //TODO add static values which I can use f.e in homecontroller!
        //-------------------------------------------------------
        configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Provider", provider },
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var connectionString = configuration.GetConnectionString(provider);

        //-------------------------------------------------------
        //Add serilog
        //-------------------------------------------------------
        if (provider == Provider.SqlServer.ToString())
        {
            var columnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn
                        {ColumnName = "Scope", PropertyName = "Scope", DataType = SqlDbType.NVarChar, DataLength = 64},
                }
            };
            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                configuration.WriteTo.Console();
                configuration.WriteTo.File("Mylogs.log", rollingInterval: RollingInterval.Day);
                configuration.WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = true,
                    TableName = "LogEvents",
                }, columnOptions: columnOptions).MinimumLevel.Information().Enrich.FromLogContext();
            });
        }
        else if (provider == Provider.PostgresWin.ToString())
        {
            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                //configuration.MinimumLevel.Information();
                //configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                //configuration.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning);
                //configuration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
                //configuration.MinimumLevel.Override("Microsoft.Hosting.AspNetCore", LogEventLevel.Warning);
                //configuration.MinimumLevel.Override("MainApp.Web", LogEventLevel.Warning);
                //configuration.WriteTo.Console();
                configuration.MinimumLevel.Verbose();
                configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                configuration.MinimumLevel.Override("System", LogEventLevel.Error);
                configuration.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}");
                configuration.WriteTo.File("Mylogs.log", rollingInterval: RollingInterval.Day);
                configuration.WriteTo.PostgreSQL(
                      connectionString: "Server = localhost; Port=5432; User Id=postgres; Password=mario13; Database=MainAppWeb;",
                      tableName: "LogEvents",
                      restrictedToMinimumLevel: LogEventLevel.Information,
                      needAutoCreateTable: true,
                      respectCase: true,
                      useCopy: false
                    );
            });
        }
        else if (provider == Provider.PostgresLinux.ToString())
        {
            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                configuration.MinimumLevel.Verbose();
                configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
                configuration.MinimumLevel.Override("System", LogEventLevel.Error);
                configuration.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}");
                configuration.WriteTo.File("./Logs/Mylogs.log", rollingInterval: RollingInterval.Day);
                configuration.WriteTo.PostgreSQL(
                      connectionString: "Server = localhost; Port=5432; User Id=mario; Password=mario13; Database=MainAppWebPg;",
                      tableName: "LogEvents",
                      restrictedToMinimumLevel: LogEventLevel.Information,
                      needAutoCreateTable: true,
                      respectCase: true,
                      useCopy: false
                    );
            });
        }
        else
        {
            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                configuration.WriteTo.Console();
            });
        }

        //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(provider)));

        switch (provider)
        {
            case "MySqlLinux":
                builder.Services.AddDbContext<ApplicationDbContext, MySqlDbContext>();
                break;

            case "MySqlWin":
                builder.Services.AddDbContext<ApplicationDbContext, MySqlDbContext>();
                break;

            case "SqlServer":
                builder.Services.AddDbContext<ApplicationDbContext, MsSqlDbContext>();
                break;

            case "SqliteServer":
                builder.Services.AddDbContext<ApplicationDbContext, SqliteDbContext>();
                break;

            case "PostgresWin":
                builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
                break;

            case "PostgresLinux":
                builder.Services.AddDbContext<ApplicationDbContext, PostgresDbContext>();
                break;

            case "UnitTests":
                //builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("MainWebDb"));
                builder.Services.AddDbContext<ApplicationDbContext, MemorySqlDbContext>();
                break;

            default:
                throw new Exception($"Unsupported provider: {provider}");
        }

        // if (connectionString.Contains("SqlServer"))
        // {
        //    builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
        // }
        // if (connectionString.Contains("Sqlite"))
        // {
        //    builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connectionString));
        // }
        // if (connectionString.Contains("PostgresWin"))
        // {
        //    builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(connectionString));
        // }
        // if (connectionString.Contains("MainAppWebPg"))
        // {
        //    builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(
        //         configuration.GetConnectionString(provider),
        //     x => x.MigrationsAssembly("PostgresServerMigrations")));
        // }

        //TODO tworzenie migracji z automatu nie wiem czy dziala to?
        //builder.Services.AddDbContext<ApplicationDbContext>(
        //options => _ = provider switch
        //{
        //    "SqliteServer" => options.UseSqlite(
        //        configuration.GetConnectionString("SqliteServer"),
        //        x => x.MigrationsAssembly("SqliteMigrations")),

        //    "SqlServer" => options.UseSqlServer(
        //        configuration.GetConnectionString("SqlServer"),
        //    x => x.MigrationsAssembly("SqlServerMigrations")),

        //    "PostgresWin" => options.UseNpgsql(
        //        configuration.GetConnectionString("PostgresServerConnection"),
        //    x => x.MigrationsAssembly("PostgresServerMigrations")),

        //    "PostgresLinux" => options.UseNpgsql(
        //        configuration.GetConnectionString("PostgresLinux"),
        //    x => x.MigrationsAssembly("PostgresServerMigrations")),

        //    "MySqlWin" => options.UseNpgsql(
        //        configuration.GetConnectionString("MySqlWin"),
        //    x => x.MigrationsAssembly("MySqlServerMigration")),

        //    _ => throw new Exception($"Unsupported provider: {provider}")
        //});

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//TODO dodane aby poprawic blad zapisu czasu utc w postgres

        //Services configuration
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ApplicationLifetime>();
        builder.Services.AddTransient<TrackingService>();
        builder.Services.AddTransient<ITrackingService, TrackingService>();
        builder.Services.AddTransient<IPersonService, UserService>();
        builder.Services.AddTransient<ITrainersService, TrainersService>();
        builder.Services.AddTransient<IRepositoryService<ApplicationUserRoleView>, UserRoleService>();
        builder.Services.AddTransient<IRepositoryService<ApplicationRoles>, RoleService>();
        builder.Services.AddTransient<EmailService>();
        builder.Services.AddHttpClient();
        //builder.Services.AddSerilog(logger);//inject serilog

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
            client.Timeout = new TimeSpan(0, 0, 60);
            client.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
        });

        builder.Services.AddAuthorization();

        if (environment.EnvironmentName != "UnitTests")//TODO gdy automapper aktywny nie dzialaja testy!!!???
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        builder.Services.AddScoped<UserManager<ApplicationUser>>();
        //builder.Services.AddScoped<RoleManager<ApplicationRoles>>();

        builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsFactory>();//TODO dodanie claimow

        //builder.Services.AddRazorPages();
        //builder.Services.ConfigureApplicationCookie(options =>
        //{
        //    options.LoginPath = $"/account/login";
        //    options.LogoutPath = $"/account/logout";
        //    options.AccessDeniedPath = $"/account/accessDenied";
        //});

        //TODO czas zalogowania po 5min odpala sie login
        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });

        #region Authorization Policy
        //AddAuthorizationPolicies(builder.Services);
        #endregion

        var app = builder.Build();//TODO w unit test tutaj odpala TestingWebAppFactory!

        //await app.StopAsync();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();//TODO tutaj powinien wejsc do OnConfiguring 
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRoles>>();
            var takeConfiguration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            if (context.Database.IsRelational())
            {
                if (context.Database.IsRelational())
                {
                    context.Database.EnsureCreated();
                    //context?.Database.Migrate();
                    await SeedData.SeedUser(context, userManager, roleManager);
                }
            }
            else
            {
                //TODO nie ralacyjna baza danych np memory msql do testow
                await SeedData.SeedUser(context, userManager, roleManager);
                await SeedData.SeedRole(context);
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

        app.UseAuthorization();

        app.UseCookiePolicy();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();//TODO TO MUSI BYC JAK CHCEM UZYWAC IDENTITY/PAGE

        app.Run();

    }

    static void AddAuthorizationPolicies(IServiceCollection services)//TODO add policy to program.cs
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdmin", policy => policy.RequireRole(Roles.Admin.ToString()));
            options.AddPolicy("RequireUser", policy => policy.RequireRole(Roles.User.ToString()));
        });
    }

    static string SelectProvider()
    {
        int num;
        var provider = Provider.PostgresWin.ToString();
        Console.Write("\nSelect database provider 1 to 4: ");
        Console.Write("\n1 - postgres");
        Console.Write("\n2 - mysql");
        Console.Write("\n3 - msql");
        Console.Write("\n4 - sqlite\n");
        var numasstring = Console.ReadLine();
        while (!int.TryParse(numasstring, out num))
        {
            Console.WriteLine("This is not a number!");
            numasstring = Console.ReadLine();
        }
        switch (num)
        {
            case 1:
                Console.Write("Selected pg \n");
                provider = Provider.PostgresWin.ToString();
                break;
            case 2:
                Console.Write("selected mysql \n");
                provider = Provider.MySqlWin.ToString();
                break;
            case 3:
                Console.Write("selected msql \n");
                provider = Provider.SqlServer.ToString();
                break;
            case 4:
                Console.Write("selected sqlite \n");
                provider = Provider.SqliteServer.ToString();
                break;
            default:
                Console.WriteLine("wrong number!\n");
                break;
        }
        return provider;
    }
}