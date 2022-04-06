using MainApp.BLL.Context;
using MainApp.BLL.Models;
using MainApp.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MainAppIntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient _httpClient;  
        protected readonly IHttpClientFactory _client;

        //Server=localhost\\SQLEXPRESS;Database=TestDb;Trusted_Connection=True;MultipleActiveResultSets=True;
        //Data Source=.\\DataBaseUser\\TestDb.db

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {                        
                        services.RemoveAll(typeof(ApplicationDbContext));
                        services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase("TestDb.db"); });
                    });
                });
            _httpClient = appFactory.CreateClient();
        }

        //protected async Task  AuthenticateAsync()
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        //}

        //private async Task<string> GetJwtAsync()
        //{
        //    var response = await _client.PostAsJsonAsync(ApiRoutes.Identity.Register, new RegisterView
        //    {
        //        Email = "",
        //        Password = ""
        //    });
        //    var registrationRespons = await response.Content.ReadAsAsync<AuthSuccessResponse>();
        //    return registrationRespons.Token;
        //}
    }
}
